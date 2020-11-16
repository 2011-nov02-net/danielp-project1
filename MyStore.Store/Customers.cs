using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyStore.Store.Exceptions;

namespace MyStore.Store
{
    /// <summary>
    /// Tracks all the customers in the system.
    /// </summary>
    /// <remarks>
    /// This is a Singleton.
    /// </remarks>
    public class Customers
    {
        private static Customers _instance;

        private 
            HashSet<Customer> AllCustomers;

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
            this.AllCustomers = new HashSet<Customer>();
        }

        //Required Functionality
        //question: what do if shared name?
        /// <summary>
        /// Tells the system that a customer exists.
        /// </summary>
        /// <param name="c">A new customer object not already registered.</param>
        /// <returns>The argument.</returns>
        public Customer RegisterCustomer(Customer c)
        {
            if (!AllCustomers.Contains(c))
            {
                AllCustomers.Add(c);
                return c;
            }
            else
            {
                throw new CustomerAlreadyExistsException("This customer already exists.");
            }
            
        }

        /// <summary>
        /// Tells the system that a customer exists.
        /// </summary>
        /// <param name="name">The name of a customer.</param>
        /// <param name="DefaultStore">(Optional) the defualt location for a customer.</param>
        /// <returns>The new customer object.</returns>
        public Customer RegisterCustomer(Name name, Location DefaultStore = null)
        {
            return RegisterCustomer(new Customer(name, DefaultStore));
        }

        /// <summary>
        /// Tells the system that a customer exists.
        /// </summary>
        /// <param name="name">The name of a customer.</param>
        /// <param name="DefaultStore">(Optional) the defualt location for a customer.</param>
        /// <returns>The new customer object.</returns>
        public Customer RegisterCustomer(Name name, string DefaultStore = null)
        {
            if(DefaultStore != null)
            {
                return RegisterCustomer(new Customer(name, Locations.Instance.GetOrRegisterLocation(DefaultStore)));
            } else
            {

                return RegisterCustomer(new Customer(name));
            }
            
        }


        /// <summary>
        /// Tells the system that a customer exists.
        /// </summary>
        /// <param name="first">The customer's first name.</param>
        /// <param name="last">The customer's last name.</param>
        /// <param name="middle">The customer's middle initial.</param>
        /// <param name="DefaultStore">The customer's home store.</param>
        /// <returns>The new customer object</returns>
        public Customer RegisterCustomer(string first, string last, char? middle = null, Location DefaultStore = null)
        {
            return RegisterCustomer(new Name(first, last, middle), DefaultStore);
        }

        //required functionality
        //get customer by name
        /// <summary>
        /// Get's a customer given their name.
        /// </summary>
        /// <param name="customerName">The Name of a customer.</param>
        /// <returns>The customer.</returns>
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

            return desired ?? throw new CustomerNotFoundException($"{customerName} not found.");
        }

        /// <summary>
        /// Uses Linq to get all customers and sort by name.
        /// </summary>
        /// <returns> A list of customers sorted by their names.</returns>
        public List<Customer>  GetAllCustomersSortedByName()
        {
            return (List<Customer>)AllCustomers.OrderBy(customer => customer.CustomerName);
        }

        /// <summary>
        /// Checks if the customer has been registered.
        /// </summary>
        /// <param name="name">Name of the customer</param>
        /// <returns>True if the customer exists in the list, otherwise false.</returns>
        public bool HasCustomer(Name name)
        {
            bool result = false;

            foreach( Customer c in AllCustomers)
            {
                if( c.CustomerName.Equals(name) )
                {
                    result =  true;
                    break;
                }
            }

            return result;
        }
    }
}
