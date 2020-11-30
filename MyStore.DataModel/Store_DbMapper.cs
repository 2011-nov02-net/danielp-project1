using System;
using System.Collections.Generic;
using System.Text;
using MyStore.Store;

namespace MyStore.DataModel
{
    class Db_StoreMapper
    {
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


        public static Store.Location MapLocationToStore(DataModel.Location DbLocation)
        {
            Store.Location StoreLocation;
            string lname = DbLocation.LocationName;
            // if not, add to model
            if (!Locations.Instance.HasLocation(lname))
            {
                StoreLocation = Store.Locations.Instance.RegisterLocation(lname);

                //get invintory
                foreach (Invintory inv in DbLocation.Invintories)
                {
                    StoreLocation.AddInventory(inv.ItemName, inv.Quantity);
                }
            }
            else
            {
                StoreLocation = Store.Locations.Instance.GetLocation(lname);
            }

            return StoreLocation;
        }

        public static Name getCustomerName(DataModel.Customer c)
        {
            return new Name(c.FirstName, c.LastName, c.MiddleInitial?[0]);
        }


    }
}
