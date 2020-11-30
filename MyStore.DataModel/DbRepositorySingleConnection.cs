﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
using MyStore.Store;
using MyStore.Store.Exceptions;

namespace MyStore.DataModel
{
    public class DbRepositorySingleConnection : IDbRepository
    {

        private readonly MyStoreDbContext _context;


        public DbRepositorySingleConnection(MyStoreDbContext context)
        {
            _context = context;
        }

        #region Customers
        /// <summary>
        /// Takes a model customer, creates a DB customer, and sends it to the database. Will register
        /// the customer with the data model if the customer isn't already.
        /// </summary>
        /// <remarks>
        /// May throw exceptions if the store name is over 100 characters, or doesn't exist in the DB.
        /// </remarks>
        /// <param name="customer">The model customer.</param>
        public void CreateCustomer(Store.Customer customer)
        {
            DataModel.Customer newcustomer = new Customer();
            newcustomer.Id = (_context.Customers.OrderByDescending(cust => cust.Id).Take(1).First().Id) + 1;
            newcustomer.LastName = customer.CustomerName.Last;
            newcustomer.FirstName = customer.CustomerName.First;

            if (customer.CustomerName.MiddleInitial != null)
            {
                newcustomer.MiddleInitial = customer.CustomerName.MiddleInitial.ToString();
            }

            if (customer.DefaultStore != null)
            {
                //TODO: limit to 100 characters.
                newcustomer.StoreLocation = customer.DefaultStore.Where;
            }

            _context.Customers.Add(newcustomer);
            _context.SaveChanges();
        }

        /// <summary>
        /// Get a customer from the db by name, and register it with the data model assuming it doesn't already
        /// exist in the model.
        /// </summary>
        /// <param name="name">The name of the customer.</param>
        /// <returns>The customer created (or returned) by the Customers object in the datamodel, or null if that customer isn't 
        /// in the Database.</returns>
        public Store.Customer GetCustomerByName(Name name)
        {
            Customer DBCustomer = GetDBCustomerByName(_context, name);

            if(DBCustomer != null)
            {
                return Db_StoreMapper.MapCustomerToStore(DBCustomer);
            } else
            {
                Console.WriteLine($"Customer, {name}, is not in the DB.");
                return null;
            }
        }

        /// <summary>
        /// Get a list of all customers in the model.
        /// </summary>
        /// <returns>An enumerable list of all customers. (Concretely a hashset) </returns>
        public IEnumerable<Store.Customer> GetCustomers()
        {
            HashSet<Store.Customer> customers = new HashSet<Store.Customer>();

            //convert and check if in model
            foreach (DataModel.Customer customer in _context.Customers.Include(cust => cust.StoreLocationNavigation))
            {
                //customers.Add();
                Store.Customer nextcustomer = Db_StoreMapper.MapCustomerToStore(customer);

                if(nextcustomer != null)
                {
                    customers.Add(nextcustomer);
                }
            }

            //return list
            return customers;
        }
        #endregion


        #region Stores
        /// <summary>
        /// Get a list of all locations from the DB, and updates the model if any are missing.
        /// </summary>
        /// <returns>List of all stores</returns>
        IEnumerable<Store.Location> IDbRepository.GetLocations()
        {
            //get all customers from DB
            HashSet<Store.Location> locations = new HashSet<Store.Location>();

            //convert and check if in model
            foreach (Location l in _context.Locations.Include(store => store.Invintories).ThenInclude(inv => inv.ItemNameNavigation))
            {
                Store.Location NewLocation = Db_StoreMapper.MapLocationToStore(l);

                locations.Add(NewLocation);
            }

            //return list
            return locations;
        }


        /// <summary>
        /// Get a specific store from the db.
        /// </summary>
        /// <param name="storeName"></param>
        /// <returns></returns>
        Store.Location IDbRepository.GetLocation(string storeName)
        {
            Location store = _context.Locations
                    .Where(str => str.LocationName == storeName)
                    .Include(store => store.Invintories)
                    .FirstOrDefault();

            if (store != null)
            {
                return Db_StoreMapper.MapLocationToStore(store);
            } else
            {
                return null;
            }
        }

        /// <summary>
        /// Intended to be used to set a store's stock equal to current stocks in DB
        /// </summary>
        /// <param name="selectedStore"></param>
        void IDbRepository.UpdateAndOverwriteStoreStocks(Store.Location selectedStore)
        {
            foreach (Invintory inv in _context.Invintories.Where(x => x.StoreLocation == selectedStore.Where))
            {
                if (inv.Quantity != selectedStore.CheckStock(inv.ItemName))
                {
                    try
                    {
                        selectedStore.SetItemStock(inv.ItemName, inv.Quantity);
                    }
                    catch (ItemNotFoundException e)
                    {
                        addMissingItems(_context);
                        selectedStore.SetItemStock(inv.ItemName, inv.Quantity);
                    }
                }
            }
        }


        #endregion


        #region Orders
        //get history (req)
        /// <summary>
        /// Gets all unique order histories involving a customer, and loads them into the model
        /// if they're not already there.
        /// </summary>
        /// <param name="c">The model's version of the customer.</param>
        /// <returns> A list of all IOrders related to the customer.</returns>
        public IEnumerable<IOrder> GetOrderHistory(Store.Customer c)
        {
            Customer customer = GetDBCustomerByName(_context, c.CustomerName);
            customer = _context.Customers
                .Where(cust => cust.Id == customer.Id)
                .Include(cust => cust.Orders)
                .ThenInclude(ord => ord.OrderItems)
                .ThenInclude(ordi => ordi.Item)
                .FirstOrDefault();


            IEnumerable<IOrder> orders = Store.Orders.Instance.GetOrdersByCustomer(c);

            foreach (Order CustomerOrder_DB in customer.Orders)
            {
                bool foundEquiv = false;
                foreach (Store.IOrder CustomerOrder_MD in orders)
                {
                    if (EquivilentOrder(CustomerOrder_MD, CustomerOrder_DB))
                    {
                        foundEquiv = true;
                        break;
                    }
                }

                if (!foundEquiv)
                {

                    ICollection<ItemCount> orderitems = new List<ItemCount>();
                    foreach (OrderItem oi in CustomerOrder_DB.OrderItems)
                    {
                        orderitems.Add(new ItemCount(oi.Quantity, oi.Item.ItemName));
                    }
                    Store.Orders.Instance.CreateAndAddPastOrder(CustomerOrder_DB.StoreLocation, Db_StoreMapper.getCustomerName(CustomerOrder_DB.Customer), CustomerOrder_DB.OrderTime, orderitems, CustomerOrder_DB.OrderTotal);
                }
            }

            orders = Store.Orders.Instance.GetOrdersByCustomer(c);
            return orders;
        }



        //get history (req)
        /// <summary>
        /// Given a model location, return all orders placed from that location.
        /// Also updates the model with any missing orders.
        /// </summary>
        /// <param name="l">The model location.</param>
        /// <returns>List of orders.</returns>
        public IEnumerable<IOrder> GetOrderHistory(Store.Location l)
        {
            Location location = _context.Locations
                            .Where(loc => loc.LocationName == l.Where)
                            .Include(cust => cust.Orders)
                            .ThenInclude(cust => cust.Customer)
                            .Include(cust => cust.Orders)
                            .ThenInclude(ord => ord.OrderItems)
                            .ThenInclude(ordi => ordi.Item)
                            .FirstOrDefault();


            IEnumerable<IOrder> orders = Store.Orders.Instance.GetOrdersByLocation(l);

            foreach (Order LocationOrder_DB in location.Orders)
            {
                bool foundEquiv = false;
                foreach (Store.IOrder LocationOrder_MD in orders)
                {
                    foundEquiv = foundEquiv || EquivilentOrder(LocationOrder_MD, LocationOrder_DB);
                    if (foundEquiv)
                    {
                        break;
                    }
                }

                if (!foundEquiv)
                {
                    Console.WriteLine("no equiv found, creating order.");
                    ICollection<ItemCount> orderitems = new List<ItemCount>();
                    foreach (OrderItem oi in LocationOrder_DB.OrderItems)
                    {
                        orderitems.Add(new ItemCount(oi.Quantity, oi.Item.ItemName));
                    }

                    Name customername = Db_StoreMapper.getCustomerName(_context.Customers
                                            .Where(dbcust => dbcust.Id == LocationOrder_DB.CustomerId).FirstOrDefault()
                                        );

                    Store.Orders.Instance.CreateAndAddPastOrder(l.Where,
                            customername,
                            LocationOrder_DB.OrderTime,
                            orderitems,
                            LocationOrder_DB.OrderTotal);
                }
            }

            orders = Store.Orders.Instance.GetOrdersByLocation(l);
            return orders;
        }


        /// <summary>
        /// Take a finalized order from the model, and convert it into a db order, adjust store invintories, 
        /// and insert it.
        /// </summary>
        /// <exception cref="NullReferenceException"> 
        /// This might throw a null reference if the store has no invintory for an item in the order in the database.
        /// </exception>
        /// <param name="o">A model order. This order should have had Finalize() called on it, as this method doesn't</param>
        public void PlaceOrder(Store.Order o)
        {
            Order newOrder = new Order();
            //default is null.
            int nextid = (_context.Orders.OrderByDescending(cust => cust.Id).FirstOrDefault()?.Id ?? 0) + 1;
            newOrder.Id = nextid;
            newOrder.StoreLocation = o.OrderLoc.Where;
            newOrder.Customer = GetDBCustomerByName(_context, o.Customer.CustomerName);
            newOrder.OrderTime = o.Time;

            decimal total = 0;
            foreach (ItemCount item in o.Items)
            {
                total += item.Count * (Decimal)item.ThisItem.cost;

                OrderItem orderItem = new OrderItem();
                orderItem.Order = newOrder;
                orderItem.Quantity = item.Count;
                orderItem.ItemId = item.ThisItem.name;
                newOrder.OrderItems.Add(orderItem);

                //change store stocks, Assumes there's already an invintory entry, otherwise throws exception.
                Invintory iv = _context.Invintories.Find(o.OrderLoc.Where, item.ThisItem.name);
                iv.Quantity -= item.Count;
            }
            newOrder.OrderTotal = total;

            _context.Orders.Add(newOrder);
            _context.SaveChanges();
        }

        #endregion



        /// <summary>
        /// Loads all the DB Items into memory
        /// </summary>
        /// <remarks>
        /// Will have uncaught exceptions if used more than once. This is probably a compramise and 
        /// impracticle for large databases but makes logic easier.
        /// </remarks>
        void IDbRepository.LoadDBDataToModel()
        {
            //get all locations -> model
            foreach (Location l in _context.Locations)
            {
                Store.Locations.Instance.RegisterLocation(l.LocationName);
            }

            //get customers -> model
            foreach (Customer c in _context.Customers)
            {
                Store.Customer newcust = Store.Customers.Instance.RegisterCustomer(Db_StoreMapper.getCustomerName(c), c.StoreLocation);

                /*
                if(c.StoreLocation != null)
                {
                    newcust.SetDefaultStore(Store.Locations.Instance.GetLocation(c.StoreLocation));
                }
                */
            }

            //get all items -> model
            foreach (Item i in _context.Items)
            {
                Store.StoreCatalogue.Instance.RegisterItem(i.ItemName, i.ItemPrice);
            }

            //get all orders -> model
            // as historic orders 
            foreach (Order o in _context.Orders.Include(oi => oi.OrderItems).ThenInclude(oi => oi.Item).Include(oi => oi.Customer))
            {
                //get all items in the order
                ICollection<ItemCount> orderItems = new List<ItemCount>();
                foreach (OrderItem orderItem in o.OrderItems)
                {
                    orderItems.Add(new ItemCount(orderItem.Quantity, orderItem.Item.ItemName));
                }

                Store.Orders.Instance.CreateAndAddPastOrder(
                    o.StoreLocation,
                    Db_StoreMapper.getCustomerName(o.Customer),
                    o.OrderTime,
                    orderItems,
                    o.OrderTotal
                    );
            }

            //get all store invintories
            foreach (Invintory i in _context.Invintories)
            {
                Locations.Instance.GetLocation(i.StoreLocation).AddInventory(i.ItemName, i.Quantity);
            }
        }




        #region Private Helpers

        /// <summary>
        /// Get the db's object for a customer
        /// </summary>
        /// <param name="DBContext">current connection</param>
        /// <param name="name">The name of the customer</param>
        /// <returns>DB customer</returns>
        private Customer GetDBCustomerByName(MyStoreDbContext DBContext, Name name)
        {
            Customer DBCustomer;
            if (name.MiddleInitial == null)
            {
                DBCustomer = DBContext.Customers.Where(cust =>
                    name.First.StartsWith(cust.FirstName)
                    && name.Last.StartsWith(cust.LastName)
                    && null == cust.MiddleInitial)
                    .SingleOrDefault();
            }
            else
            {
                DBCustomer = DBContext.Customers.Where(cust =>
                    name.First.StartsWith(cust.FirstName)
                    && name.Last.StartsWith(cust.LastName)
                    && name.MiddleInitial.ToString() == cust.MiddleInitial)
                    .Take(1).SingleOrDefault();
            }

            return DBCustomer;
        }



       

        /// <summary>
        /// Check if a store order is equivilent with a model order
        /// </summary>
        /// <param name="storder"> A Model order</param>
        /// <param name="modelOrder">A DB order</param>
        /// <returns>True if an order is the same, otherwise false.</returns>
        private bool EquivilentOrder(Store.IOrder storder, Order modelOrder)
        {
            bool result = true;

            //compare store
            if (storder.OrderLoc.Where != modelOrder.StoreLocation)
            {
                result = false;

                return result;
            }
            //compare customer
            if (!(new Name(modelOrder.Customer.FirstName, modelOrder.Customer.LastName, modelOrder.Customer.MiddleInitial?[0])
                .Equals(storder.Customer.CustomerName)))
            {
                result = false;
                return result;
            }

            if (storder.Cost != modelOrder.OrderTotal)
            {
                result = false;
                return result;
            }

            //compare time with some leeway
            if (Math.Abs((storder.Time - modelOrder.OrderTime).TotalMinutes) > 45)
            {
                result = false;
                return result;
            }

            //compare order items
            foreach (OrderItem oi in modelOrder.OrderItems)
            {
                bool foundmatch = false;
                foreach (Store.ItemCount ic in storder.Items)
                {
                    if (ic.ThisItem.name == oi.Item.ItemName && ic.Count == oi.Quantity)
                    {
                        foundmatch = true;
                        break;
                    }
                }

                if (!foundmatch)
                {
                    result = false;
                    return result;
                }
            }

            return result;
        }


        /// <summary>
        /// Use the location to get the invintory added to the model.
        /// </summary>
        /// <remarks>
        /// ONLY for use when initializing a store
        /// </remarks>
        /// <param name="location">The location to fetch and add all stocks to</param>
        private void AddStockToModel(MyStoreDbContext context, Location location)
        {

            Store.Location modelLoc = Store.Locations.Instance.GetLocation(location.LocationName);
            foreach (Invintory invintory in context.Invintories
                .Where(inv => inv.StoreLocation == location.LocationName))
            {
                modelLoc.AddInventory(invintory.ItemName, invintory.Quantity);
            }
        }

        /// <summary>
        /// Add any missing items to the model.
        /// </summary>
        /// <param name="context"></param>
        private void addMissingItems(MyStoreDbContext context)
        {
            foreach (Item i in context.Items)
            {
                if (!StoreCatalogue.Instance.ItemExists(i.ItemName))
                {
                    Store.StoreCatalogue.Instance.RegisterItem(i.ItemName, i.ItemPrice);
                }
            }
        }

        
        #endregion
    }
}