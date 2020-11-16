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


        public void CreateCustomer(Store.Customer customer)
        {
            throw new NotImplementedException();
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

            Customer DBCustomer;
            if (name.MiddleInitial == null)
            {
                DBCustomer = DBContext.Customers.Where(cust =>
                    name.First.StartsWith(cust.FirstName)
                    && name.Last.StartsWith(cust.LastName)
                    && null == cust.MiddleInitial)
                    .SingleOrDefault();
            } else
            {
                DBCustomer = DBContext.Customers.Where(cust =>
                    name.First.StartsWith(cust.FirstName)
                    && name.Last.StartsWith(cust.LastName)
                    && name.MiddleInitial.ToString() == cust.MiddleInitial)
                    .Take(1).SingleOrDefault();
            }

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
            throw new NotImplementedException();
        }

        public IEnumerable<IOrder> GetOrderHistory(Store.Customer c)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IOrder> GetOrderHistory(Store.Location l)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ItemCount> GetStoreStocks(Store.Location l)
        {
            throw new NotImplementedException();
        }

        public void PlaceOrder(Order o)
        {
            throw new NotImplementedException();
        }

        public void UpdateOrder(Order o)
        {
            throw new NotImplementedException();
        }


        private Project0DBContext ConnectToDB()
        {
            return new Project0DBContext(this.dbContextOptions);
        }
    }
}
