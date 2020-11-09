using System;
using System.Collections.Generic;
using System.Linq;
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
        //question: what do if shared name?
        //TODO: add new customers
        public Customer RegisterCustomer(Customer c)
        {
            throw new NotImplementedException();
        }


        public Customer RegisterCustomer(Name name, Location DefaultStore = null)
        {
            return RegisterCustomer(new Customer(name, DefaultStore));
        }

        //required functionality
        //get customer by name
        public Customer GetCustomer(Name customerName)
        {
            Customer desired = null;
            foreach(Customer c in AllCustomers)
            {
                if (c.CustomerName.Equals(customerName))
                {
                    desired = c;
                    break;
                }
            }

            return desired ?? throw new ArgumentException();
        }

        /// <summary>
        /// Uses Linq to get all customers and sort by name.
        /// </summary>
        /// <returns> A list of customers sorted by their names.</returns>
        public List<Customer>  GetAllCustomersSortedByName()
        {
            return (List<Customer>)AllCustomers.OrderBy(customer => customer.CustomerName);
        }
    }
}
