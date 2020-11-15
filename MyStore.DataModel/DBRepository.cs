using System;
using System.Collections.Generic;
using System.Text;
using MyStore.Store;

namespace MyStore.DataModel
{
    class DBRepository : IDbRepository
    {
        public void CreateCustomer(Store.Customer customer)
        {
            throw new NotImplementedException();
        }

        public Store.Customer GetCustomerByName()
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
    }
}
