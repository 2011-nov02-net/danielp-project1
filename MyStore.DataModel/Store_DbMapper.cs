using System;
using System.Collections.Generic;
using System.Text;
using MyStore.Store;
using MyStore.Store.Exceptions;

namespace MyStore.DataModel
{
    static class Db_StoreMapper
    {
        /// <summary>
        /// Takes a datamodel customer and turns it into a store customer.
        /// </summary>
        /// <param name="DbCustomer">A data model customer to be added to the model</param>
        /// <returns>A Model customer</returns>
        public static Store.Customer MapCustomerToStore(DataModel.Customer DbCustomer)
        {
            if (DbCustomer != null)
            {
                Name name = Db_StoreMapper.getCustomerName(DbCustomer);
                if (Customers.Instance.HasCustomer(name))
                {
                    //something weird happened probably. Expecting customers to be gotten from 
                    //the model first before checking DB.
                    Console.Error.WriteLine($"Warning: Customer {name} already existed in the model");
                    return Customers.Instance.GetCustomer(name);
                }
                else
                {
                    if (DbCustomer.StoreLocation != null)
                    {
                        return Customers.Instance.RegisterCustomer(name, Locations.Instance.GetOrRegisterLocation(DbCustomer.StoreLocation));
                    }
                    return Customers.Instance.RegisterCustomer(name, DbCustomer.StoreLocation);
                }
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Takes a DB Location and maps it to a store Location. Will add
        /// to the model if not already there.
        /// </summary>
        /// <param name="DbLocation">The DB representation of the location.</param>
        /// <returns>The Model representation of the location.</returns>
        public static Store.Location MapLocationToStore(DataModel.Location DbLocation)
        {
            Store.Location StoreLocation;
            string lname = DbLocation.LocationName;
            // if not, add to model
            if (!Locations.Instance.HasLocation(lname))
            {
                StoreLocation = Locations.Instance.RegisterLocation(lname);

                //get invintory
                foreach (Invintory inv in DbLocation.Invintories)
                {
                    try
                    {
                        StoreLocation.AddInventory(inv.ItemName, inv.Quantity);
                    } catch(ItemNotFoundException e)
                    {
                        Console.WriteLine(e.Message);
                        StoreCatalogue.Instance.RegisterItem(inv.ItemName, inv.ItemNameNavigation.ItemPrice);
                        StoreLocation.AddInventory(inv.ItemName, inv.Quantity);
                    }
                }
            }
            else
            {
                StoreLocation = Store.Locations.Instance.GetLocation(lname);
            }

            return StoreLocation;
        }



        /// <summary>
        /// Create a model order and add it to the Model based of a DB order
        /// </summary>
        /// <remarks>
        /// Assumes that this order is confirmed unique by the caller, and should be added.
        /// Will be added as a Historic order. Takes a name so that all the customers details will
        /// be recoreded if they are not in the Model.
        /// </remarks>
        /// <param name="LocationOrder_DB">The DB order item</param>
        /// <param name="customername">The name of the customer placing the order</param>
        public static void MapAndAddOrderToModel(Order LocationOrder_DB)
        {

            ICollection<ItemCount> orderitems = new List<ItemCount>();
            foreach (OrderItem oi in LocationOrder_DB.OrderItems)
            {
                try
                {
                    orderitems.Add(new ItemCount(oi.Quantity, oi.Item.ItemName));
                }
                catch (ItemNotFoundException e)
                {
                    Console.WriteLine(e.Message);

                    StoreCatalogue.Instance.RegisterItem(oi.Item.ItemName, oi.Item.ItemPrice);
                    // retry 
                    orderitems.Add(new ItemCount(oi.Quantity, oi.Item.ItemName));
                }

            }           

            try
            {
                Store.Orders.Instance.CreateAndAddPastOrder(LocationOrder_DB.StoreLocation,
                    getCustomerName(LocationOrder_DB.Customer),
                    LocationOrder_DB.OrderTime,
                    orderitems,
                    LocationOrder_DB.OrderTotal);
            } catch (CustomerNotFoundException e)
            {
                Console.WriteLine(e.Message);
                //load the customer to the store
                Store.Customer storecust = MapCustomerToStore(LocationOrder_DB.Customer);
                //try and load the order to the model
                Store.Orders.Instance.CreateAndAddPastOrder(LocationOrder_DB.StoreLocation,
                    storecust.CustomerName,
                    LocationOrder_DB.OrderTime,
                    orderitems,
                    LocationOrder_DB.OrderTotal);
            }
            
        }



        public static Name getCustomerName(DataModel.Customer c)
        {
            return new Name(c.FirstName, c.LastName, c.MiddleInitial?[0]);
        }


    }
}
