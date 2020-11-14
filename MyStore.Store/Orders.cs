using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace MyStore.Store
{
    //TODO: Replace order history classes.
    /// <summary>
    /// singleton
    /// </summary>
    class Orders
    {
        private List<IOrder> AllOrders;
        private static Orders _instance;

        public static Orders Instance
        {
            get
            {
                if(_instance == null)
                {
                    _instance = new Orders();
                }
                return _instance;
            }
        }

        private Orders()
        {
            AllOrders = new List<IOrder>();
        }

        public IEnumerable<IOrder> GetOrdersByCustomer(Customer c)
        {
            return AllOrders.AsReadOnly().Where(ord => ord.Customer == c); 
        }

        public IEnumerable<IOrder> GetOrdersByLocation(Location l)
        {
            return AllOrders.AsReadOnly().Where(ord => ord.OrderLoc == l);
        }
    }
}
}
