using System;
using System.Collections.Generic;
using System.Linq;

namespace MyStore.Store
{
    /// <summary>
    /// singleton
    /// </summary>
    public class Orders
    {       
        #region Singleton
        private static Orders _instance;

        /// <summary>
        /// Accsess the single instance of the Orders class, responsible for tracking all orders.
        /// </summary>
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

        /// <summary>
        /// singleton constructor
        /// </summary>
        private Orders()
        {
            AllOrders = new List<IOrder>();
        }
        #endregion

        /// <summary>
        /// The list of all orders.
        /// </summary>
        private List<IOrder> AllOrders;

        /// <summary>
        /// Returns a list of orders filtered by customer.
        /// </summary>
        /// <param name="c"> The customer who's orders you want.</param>
        /// <returns>Read only list of orders connected to that customer</returns>
        public IEnumerable<IOrder> GetOrdersByCustomer(Customer c)
        {
            return AllOrders.AsReadOnly().Where(ord => ord.Customer == c); 
        }

        /// <summary>
        /// Returns a list of orders filtered by location
        /// </summary>
        /// <param name="l">The location associated with the desired orders.</param>
        /// <returns>Read only list of orders.</returns>
        public IEnumerable<IOrder> GetOrdersByLocation(Location l)
        {
            return AllOrders.AsReadOnly().Where(ord => ord.OrderLoc.LocationName == l.LocationName);
        }

        /// <summary>
        /// Gets an order by it's ID
        /// </summary>
        /// <param name="id">The ID of the order</param>
        /// <returns>The order with the matching ID or null.</returns>
        public IOrder GetOrderByID(int id)
        {
            return AllOrders
                .Where(order => order.ID == id)
                .FirstOrDefault();
        }


        /// <summary>
        /// Add a new order to the list of orders.
        /// </summary>
        /// <param name="newo">The new order to be added to the list.</param>
        public void AddOrders(IOrder newo)
        {
            AllOrders.Add(newo);
        }

        /// <summary>
        /// Create and adds a new historic order representing a completed and validated order
        /// that has been made before the model was loaded.
        /// NOTE: this bypassis validity checks, and should only be used for past orders.
        /// </summary>
        /// <param name="locationName">The name of the location</param>
        /// <param name="customerName">The name of the customer</param>
        /// <param name="time">Time the order was Placed</param>
        /// <param name="items">Items in the order</param>
        public void CreateAndAddPastOrder(string locationName, Name customerName, DateTime time, ICollection<ItemCount> items, decimal cost, int ID )
        {
            AllOrders.Add(new HistoricOrder(locationName, customerName, time, items, cost, ID));
        }

        /// <summary>
        /// Get all orders the model knows of
        /// </summary>
        /// <returns> A read only collection of all orders.</returns>
        public IEnumerable<IOrder> GetAllOrders()
        {
            return AllOrders.AsReadOnly();
        }    
    }
}

