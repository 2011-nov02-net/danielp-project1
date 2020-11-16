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

            if (Customers.Instance.HasCustomer(name))
            {
                //something weird happened probably. Expecting customers to be gotten from 
                //the model first before checking DB.
                Console.Error.WriteLine($"Warning: Customer {name} already existed in the model");
                return Customers.Instance.GetCustomer(name);
            } else
            {
                if(DBCustomer.StoreLocation != null)
                {
                    return Customers.Instance.RegisterCustomer(name, Locations.Instance.GetOrRegisterLocation(DBCustomer.StoreLocation));
                }
                return Customers.Instance.RegisterCustomer(name, Locations.Instance.GetOrRegisterLocation(DBCustomer.StoreLocation));
            }
        }

        public IEnumerable<Store.Customer> GetCustomers()
        {
            //get all customers from DB

            //convert and check if in model
            // if not, add to model

            //return list
            throw new NotImplementedException();
        }

        //TODO: get history (req)
        public IEnumerable<IOrder> GetOrderHistory(Store.Customer c)
        {
            throw new NotImplementedException();
        }
        
        //TODO: get history (req)
        public IEnumerable<IOrder> GetOrderHistory(Store.Location l)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ItemCount> GetStoreStocks(Store.Location l)
        {
            throw new NotImplementedException();
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
                Store.Customer newcust = Store.Customers.Instance.RegisterCustomer(
                    new Name(c.FirstName, c.LastName, c?.MiddleInitial?[0]));

                if(c.StoreLocation != null)
                {
                    newcust.SetDefaultStore(Store.Locations.Instance.GetLocation(c.StoreLocation));
                }
            }

            //get all items -> model
            foreach (Item i in context.Items)
            {
                Store.StoreCatalogue.Instance.RegisterItem(i.ItemName, (float) i.ItemPrice);
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
                    orderItems
                    );               
            }

            //get all store invintories
            throw new NotImplementedException();
        }

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

    }
}
