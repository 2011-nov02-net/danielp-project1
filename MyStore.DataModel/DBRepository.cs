using System;
using System.Collections.Generic;
using System.Text;
using MyStore.Store;
using Microsoft.EntityFrameworkCore;


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

        public Store.Customer GetCustomerByName(Name name)
        {
            throw new NotImplementedException();
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
