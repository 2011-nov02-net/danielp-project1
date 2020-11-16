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
    public class Orders
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

        /// <summary>
        /// Add a new order to the list of orders.
        /// </summary>
        /// <param name="newo"></param>
        public void AddOrders(IOrder newo)
        {
            AllOrders.Add(newo);
        }

        /// <summary>
        /// Create and adds a new historic order representing a completed and validated order.
        /// NOTE: this bypassis validity checks, and should only be used for past orders.
        /// </summary>
        /// <param name="locationName"></param>
        /// <param name="customerName"></param>
        /// <param name="time"></param>
        /// <param name="items"></param>
        public void CreateAndAddPastOrder(string locationName, Name customerName, DateTime time, ICollection<ItemCount> items, decimal cost )
        {
            AllOrders.Add(new HistoricOrder(locationName, customerName, time, items, cost));
        }
    }
}

