using System;
using System.Collections.Generic;
using System.Text;

namespace MyStore.Store
{
    /// <summary>
    /// Tracks all the customers in the system.
    /// </summary>
    /// <remarks>
    /// Singleton
    /// </remarks>
    class Customers
    {
        private static Customers _instance;

        private List<Customer> AllCustomers;

        public static Customers Instance
        {
            get
            {
                if( _instance is null)
                {
                    _instance = new Customers();
                }

                return _instance;
            }
        }

        private Customers()
        {
            this.AllCustomers = new List<Customer>();
        }

        //Required Functionality
        //TODO: add new customers
        public void AddCustomer(Customer c)
        {
            throw new NotImplementedException();
        }

        //required functionality
        //TODO: get customer by name
        public List<Customer> GetCustomer(Name customerName)
        {
            throw new NotImplementedException();
        }
    }
}
