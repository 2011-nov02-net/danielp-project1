using System.Collections.Generic;

namespace MyStore.Store
{
    /// <summary>
    /// A class that represents a customer who shops at the store.
    /// </summary>
    public class Customer
    {
        //order history
        /// <summary>
        /// Gets the order history related to this customer from the model.
        /// </summary>
        public IEnumerable<IOrder> CustomerOrderHistory 
        {
            get
            {
                return Orders.Instance.GetOrdersByCustomer(this);
            } 
        }

        /// <summary>
        /// The name of the customer.
        /// </summary>
        public readonly Name CustomerName;

        //NOTE: optional req
        /// <summary>
        /// The default store for a customer.
        /// </summary>
        #nullable enable
        public Location? DefaultStore { get; private set; } = null;

       
        /// <summary>
        /// Creates a new customer.
        /// </summary>
        /// <param name="name">The customer's name.</param>
        /// <param name="DefaultStore">Optional default store for the customer to use.</param>
        internal Customer(Name name, Location? DefaultStore = null)
        {
            CustomerName = name;
            this.DefaultStore = DefaultStore;
        }

        /// <summary>
        /// Set's the customer's default store.
        /// </summary>
        /// <param name="l">The location that represents the store.</param>
        public void SetDefaultStore(Location l)
        {
            this.DefaultStore = l;
        }

        /// <summary>
        /// Check if one customer is equal to another, based on the name
        /// </summary>
        /// <param name="obj"> Another object.</param>
        /// <returns>True if the customers are equivilent.</returns>
        public override bool Equals(object obj)
        {
            if(obj is Customer)
            {
                Customer other = (Customer)obj;
                return CustomerName.Equals(other.CustomerName);
            } else
            {
                return false;
            }           
        }

        /// <summary>
        /// Converts the customer into a string appropriate for console printing.
        /// </summary>
        /// <returns>A string representation of the customer.</returns>
        public override string ToString()
        {
            string customerstring = CustomerName.ToString();
            if (DefaultStore != null)
            {
                customerstring += "\t HomeStore: " + DefaultStore.LocationName;
            }
            return customerstring;
        }
    }
}