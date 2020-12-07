﻿using System;
using System.Collections.Generic;
using System.Text;

namespace MyStore.Store
{
    /// <summary>
    /// An interface for any object representing an order to implement.
    /// </summary>
    /// <seealso cref="Order"/>
    public interface IOrder 
    {
        /// <summary>
        /// The store at which an order was placed at.
        /// </summary>
        public Location OrderLoc { get; }
        /// <summary>
        /// The customer who placed the order.
        /// </summary>
        public Customer Customer { get; }

        /// <summary>
        /// The time the order was placed.
        /// </summary>
        public DateTime Time { get; }

        /// <summary>
        /// A unique identifier for orders.
        /// </summary>
        public int ID { get; }

        //items and amount, optionally any price modifyer too for sales
        //must reject unreasonable number of items.
        /// <summary>
        /// The list of items and amount of them in the order.
        /// </summary>
        public ICollection<ItemCount> Items { get; }

        /// <summary>
        /// The total cost of an order.
        /// </summary>
        public decimal Cost { get; }
    }
}
