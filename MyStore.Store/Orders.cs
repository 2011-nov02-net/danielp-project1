﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace MyStore.Store
{
    /// <summary>
    /// singleton
    /// </summary>
    public class Orders
    {
        private List<IOrder> AllOrders;
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

        private Orders()
        {
            AllOrders = new List<IOrder>();
        }

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
            return AllOrders.AsReadOnly().Where(ord => ord.OrderLoc.Where == l.Where);
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

