﻿using System.Collections.Generic;

namespace MyStore.Store
{
    public class Customer
    {
        //order history
        public IEnumerable<IOrder> CustomerOrderHistory 
        {
            get
            {
                return Orders.Instance.GetOrdersByCustomer(this);
            } 
        }

        public readonly Name CustomerName;

        //NOTE: optional req
        #nullable enable
        public Location? DefaultStore { get; private set; } = null;

       

        internal Customer(Name name, Location? DefaultStore = null)
        {
            CustomerName = name;
            this.DefaultStore = DefaultStore;
        }


        public void SetDefaultStore(Location l)
        {
            this.DefaultStore = l;
        }

        public override bool Equals(object obj)
        {
            if( obj is Customer)
            {
                Customer other = (Customer)obj;
                return this.CustomerName.Equals(other.CustomerName);
            } else
            {
                return base.Equals(obj);
            }           
        }


        public override string ToString()
        {
            string customerstring = CustomerName.ToString();
            if (DefaultStore != null)
            {
                customerstring += "\t HomeStore: " + DefaultStore.Where;
            }
            return customerstring;
        }
    }
}