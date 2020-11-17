using System;
using System.Collections.Generic;
using System.Text;
using MyStore.Store;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace MyStore.DataModel
{
    public class DBRepository : IDbRepository
    {
        private DbContextOptions<Project0DBContext> dbContextOptions;

        public DBRepository (DbContextOptions<Project0DBContext> dbContextOptions)
        {
            this.dbContextOptions = dbContextOptions;
        }

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
            using Project0DBContext DBContext = new Project0DBContext(dbContextOptions);
            if (!Customers.Instance.HasCustomer(customer.CustomerName))
            {
                Customers.Instance.RegisterCustomer(customer);
            }

            DataModel.Customer newcustomer = new Customer();
            newcustomer.Id = (DBContext.Customers.OrderByDescending(cust => cust.Id).Take(1).First().Id) + 1;
            newcustomer.LastName = customer.CustomerName.Last;
            newcustomer.FirstName = customer.CustomerName.First;

            if(customer.CustomerName.MiddleInitial != null)
            {
                newcustomer.MiddleInitial = customer.CustomerName.MiddleInitial.ToString();
            }
            
            if(customer.DefaultStore != null)
            {
                //TODO: limit to 100 characters.
                newcustomer.StoreLocation = customer.DefaultStore.Where;
            }

            DBContext.Customers.Add(newcustomer);
            DBContext.SaveChanges();
        }

        /// <summary>
        /// Get a customer from the db by name, and register it with the data model assuming it doesn't already
        /// exist in the model.
        /// </summary>
        /// <param name="name">The name of the customer.</param>
        /// <returns>The customer created (or returned) by the Customers object in the datamodel.</returns>
        public Store.Customer GetCustomerByName(Name name)
        {
            using Project0DBContext DBContext = new Project0DBContext(dbContextOptions);

            Customer DBCustomer = GetDBCustomerByName(DBContext, name);

            if(DBCustomer != null)
            {
                if (Customers.Instance.HasCustomer(name))
                {
                    //something weird happened probably. Expecting customers to be gotten from 
                    //the model first before checking DB.
                    Console.Error.WriteLine($"Warning: Customer {name} already existed in the model");
                    return Customers.Instance.GetCustomer(name);
                }
                else
                {
                    if (DBCustomer.StoreLocation != null)
                    {
                        return Customers.Instance.RegisterCustomer(name, Locations.Instance.GetOrRegisterLocation(DBCustomer.StoreLocation));
                    }
                    return Customers.Instance.RegisterCustomer(name, Locations.Instance.GetOrRegisterLocation(DBCustomer.StoreLocation));
                }
            } else
            {
                return null;
            }      
        }

        /// <summary>
        /// Get a list of all customers in the model.
        /// </summary>
        /// <returns>An enumerable list of all customers. (Concretely a hashset) </returns>
        public IEnumerable<Store.Customer> GetCustomers()
        {
            //get all customers from DB
            using Project0DBContext context = this.ConnectToDB();

            HashSet<Store.Customer> customers = new HashSet<Store.Customer>();

            //convert and check if in model
            foreach(Customer customer in context.Customers)
            {
                Name cname = this.getCustomerName(customer);
                // if not, add to model
                if (!Customers.Instance.HasCustomer(cname))
                {
                    Store.Customer NewCustomer = Store.Customers.Instance.RegisterCustomer(cname, customer.StoreLocation);
                } else
                {
                    customers.Add(Store.Customers.Instance.GetCustomer(cname));
                }
            }

            //return list
            return customers;
        }


        IEnumerable<Store.Location> IDbRepository.GetLocations()
        {
            //get all customers from DB
            using Project0DBContext context = this.ConnectToDB();

            HashSet<Store.Location> locations = new HashSet<Store.Location>();

            //convert and check if in model
            foreach (Location l in context.Locations)
            {
                string lname = l.LocationName;
                // if not, add to model
                if (!Locations.Instance.HasLocation(lname))
                {
                    Store.Location NewLocation = Store.Locations.Instance.RegisterLocation(lname);
                    //get invintory
                    AddStockToModel(context, l);
                    locations.Add(NewLocation);
                }
                else
                {
                    locations.Add(Store.Locations.Instance.GetLocation(lname));
                }
            }

            //return list
            return locations;
        }

        //get history (req)
        /// <summary>
        /// Gets all unique order histories involving a customer, and loads them into the model
        /// if they're not already there.
        /// </summary>
        /// <param name="c">The model's version of the customer.</param>
        /// <returns> A list of all IOrders related to the customer.</returns>
        public IEnumerable<IOrder> GetOrderHistory(Store.Customer c)
        {
            Project0DBContext dBContext = this.ConnectToDB();

            Customer customer = GetDBCustomerByName(dBContext, c.CustomerName);
            customer = dBContext.Customers
                .Where(cust => cust.Id == customer.Id)
                .Include(cust => cust.Orders)
                .ThenInclude(ord => ord.OrderItems)
                .ThenInclude(ordi => ordi.Item)
                .FirstOrDefault();


            IEnumerable<IOrder> orders = Store.Orders.Instance.GetOrdersByCustomer(c);

            foreach (Order CustomerOrder_DB in customer.Orders)
            {
                bool foundEquiv = false;
                foreach(Store.IOrder CustomerOrder_MD in orders)
                {
                    if (!this.EquivilentOrder(CustomerOrder_MD, CustomerOrder_DB))
                    {
                        foundEquiv = true;
                        break;
                    }
                }

                if (!foundEquiv)
                {

                    ICollection<ItemCount> orderitems = new List<ItemCount>();
                    foreach(OrderItem oi in CustomerOrder_DB.OrderItems)
                    {
                        orderitems.Add(new ItemCount(oi.Quantity, oi.Item.ItemName));
                    }
                    Store.Orders.Instance.CreateAndAddPastOrder(CustomerOrder_DB.StoreLocation, this.getCustomerName(CustomerOrder_DB.Customer), CustomerOrder_DB.OrderTime, orderitems, CustomerOrder_DB.OrderTotal);
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
            Project0DBContext dBContext = this.ConnectToDB();  

            Location location = dBContext.Locations
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
                    foundEquiv = EquivilentOrder(LocationOrder_MD, LocationOrder_DB);
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

                    Name customername = getCustomerName(dBContext.Customers
                                            .Where(dbcust => dbcust.Id  == LocationOrder_DB.CustomerId).FirstOrDefault()
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



        public IEnumerable<ItemCount> GetStoreStocks(Store.Location l)
        {
            IEnumerable<ItemCount> ModdelStock = l.GetLocationStock();
            //may have uncommitted changes in an asynchronus enviorment
            //no way to reconsile stock changes, and there shouldn't be any discrepency ...
            //so just returning
            return ModdelStock;
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
            using Project0DBContext DBContext = new Project0DBContext(dbContextOptions);

            Order newOrder = new Order();
            //default is null.
            int nextid = (DBContext.Customers.OrderByDescending(cust => cust.Id).FirstOrDefault()?.Id ?? -1 ) + 1;
            newOrder.Id = nextid;
            newOrder.StoreLocation = o.OrderLoc.Where;
            newOrder.Customer = GetDBCustomerByName(DBContext, o.Customer.CustomerName);
            newOrder.OrderTime = o.Time;

            decimal total = 0;
            foreach (ItemCount item in o.Items)
            {
                total += item.Count * (Decimal) item.ThisItem.cost;

                OrderItem orderItem = new OrderItem();
                orderItem.Order = newOrder;
                orderItem.Quantity = item.Count;
                orderItem.ItemId = item.ThisItem.name;
                newOrder.OrderItems.Add(orderItem);

                //change store stocks, Assumes there's already an invintory entry, otherwise throws exception.
                Invintory iv = DBContext.Invintories.Find(new {o.OrderLoc.Where, item.ThisItem.name}) ;
                iv.Quantity -= item.Count;
            }
            newOrder.OrderTotal = total;

            DBContext.Orders.Add(newOrder);
            DBContext.SaveChanges();
        }



        public void UpdateOrder(Store.Order o)
        {
            throw new NotImplementedException();
        }



        /// <summary>
        /// Loads all the DB Items into memory
        /// </summary>
        /// <remarks>
        /// Will have uncaught exceptions if used more than once. This is probably a compramise and 
        /// impracticle for large databases but makes logic easier.
        /// </remarks>
        void IDbRepository.LoadDBDataToModel()
        {
            using Project0DBContext context = ConnectToDB();
            //get all locations -> model
            foreach(Location l in context.Locations)
            {
                Store.Locations.Instance.RegisterLocation(l.LocationName);
            }

            //get customers -> model
            foreach(Customer c in context.Customers)
            {
                Store.Customer newcust = Store.Customers.Instance.RegisterCustomer(getCustomerName(c), c.StoreLocation);

                /*
                if(c.StoreLocation != null)
                {
                    newcust.SetDefaultStore(Store.Locations.Instance.GetLocation(c.StoreLocation));
                }
                */
            }

            //get all items -> model
            foreach (Item i in context.Items)
            {
                Store.StoreCatalogue.Instance.RegisterItem(i.ItemName,  i.ItemPrice);
            }

            //get all orders -> model
            // as historic orders 
            foreach(Order o in context.Orders.Include(oi => oi.OrderItems).ThenInclude(oi => oi.Item).Include(oi => oi.Customer))
            {
                //get all items in the order
                ICollection<ItemCount> orderItems = new List<ItemCount>();
                foreach( OrderItem orderItem in o.OrderItems)
                {
                    orderItems.Add(new ItemCount(orderItem.Quantity, orderItem.Item.ItemName));
                }

                Store.Orders.Instance.CreateAndAddPastOrder(
                    o.StoreLocation,
                    getCustomerName(o.Customer),
                    o.OrderTime,
                    orderItems,
                    o.OrderTotal
                    );               
            }

            //get all store invintories
            foreach(Invintory i in context.Invintories)
            {
                Locations.Instance.GetLocation(i.StoreLocation).AddInventory(i.ItemName, i.Quantity);
            }
        }

        /// <summary>
        /// Get a specific store from the db.
        /// </summary>
        /// <param name="storeName"></param>
        /// <returns></returns>
        Store.Location IDbRepository.GetLocation(string storeName)
        {
            using Project0DBContext context = ConnectToDB();
            Location store = context.Locations
                    .Where(str => str.LocationName == storeName)
                    .Include(str => str.Invintories)
                    .FirstOrDefault();

            if(store != null)
            {
                if (Locations.Instance.HasLocation(storeName))
                {
                    return Locations.Instance.GetLocation(storeName);
                }
                else
                {
                    //create location
                    Store.Location newLocation = Locations.Instance.RegisterLocation(storeName);
                    //add invintory
                    foreach(Invintory inv in store.Invintories)
                    {
                        newLocation.AddInventory(inv.ItemName, inv.Quantity);
                    }
                    return newLocation;
                }
            } else
            {
                return null;
            }          
        }


        /// <summary>
        /// Connect to the database.
        /// </summary>
        /// <returns>A new DB context</returns>
        private Project0DBContext ConnectToDB()
        {
            return new Project0DBContext(this.dbContextOptions);
        }



        /// <summary>
        /// Get the db's object for a customer
        /// </summary>
        /// <param name="DBContext">current connection</param>
        /// <param name="name">The name of the customer</param>
        /// <returns>DB customer</returns>
        private Customer GetDBCustomerByName(Project0DBContext DBContext, Name name)
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



        private Name getCustomerName(Customer c)
        {
            return new Name(c.FirstName, c.LastName, c.MiddleInitial?[0]);
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
            if(storder.OrderLoc.Where != modelOrder.StoreLocation)
            {
                result = false;
                return result;
            }

            //compare customer
            if(! (new Name(modelOrder.Customer.FirstName, modelOrder.Customer.LastName, modelOrder.Customer.MiddleInitial?[0])
                .Equals(storder.Customer.CustomerName)))
            {
                result = false;
                return result;
            }

            if(storder.Cost !=  modelOrder.OrderTotal)
            {
                result = false;
                return result;
            }

            //compare time with some leeway
            if (Math.Abs(storder.Time.Ticks - modelOrder.OrderTime.Ticks) > 1000)
            {
                result = false;
                return result;
            }

            //compare order items
            foreach(OrderItem oi in modelOrder.OrderItems)
            {
                foreach(Store.ItemCount ic in storder.Items)
                {
                    if(ic.ThisItem.name == oi.Item.ItemName && ic.Count == oi.Quantity)
                    {
                        continue;
                    } else
                    {
                        result = false;
                        return result;
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Use the location to get the invintory added to the model.
        /// </summary>
        /// <param name="location"></param>
        private void AddStockToModel(Project0DBContext context, Location location)
        {

            Store.Location modelLoc = Store.Locations.Instance.GetLocation(location.LocationName);
            foreach(Invintory invintory in context.Invintories
                .Where(inv => inv.StoreLocation == location.LocationName))
            {
                modelLoc.AddInventory(invintory.ItemName, invintory.Quantity);
            }
        }
    }
}
