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

        //items and amount, optionally any price modifyer too for sales
        //must reject unreasonable number of items.
        public ICollection<ItemCount> Items { get; }

        public decimal Cost { get; }


        /// <summary>
        /// Finalizes the order and adds it to the customer and location's order history.
        /// MAY also adjust the stocks of the store.
        /// </summary>
        //public void FinallizeOrder();
    }
}
