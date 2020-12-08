using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyStore.Store
{
    internal class HistoricOrder : IOrder
    {
        /// <summary>
        /// Where the order was placed.
        /// </summary>
        public Location OrderLoc { get; }

        /// <summary>
        /// The customer who placed the order
        /// </summary>
        public Customer Customer { get; }

        /// <summary>
        /// The Time the order was placed
        /// </summary>
        public DateTime Time { get; }
        
        /// <summary>
        /// The Items in the order
        /// </summary>
        public ICollection<ItemCount> Items { get; }

        /// <summary>
        /// The total cost of the order at the time it was placed
        /// </summary>
        public decimal Cost { get;  }

        /// <summary>
        /// The ID of the order. A unique identifier.
        /// </summary>
        public int ID { get; }

        /// <summary>
        /// Allows you to add an item to the historic order.
        /// </summary>
        /// <param name="itemname">The name of the item to add to the order</param>
        /// <param name="amount">Amount of the item to be added to the order</param>
        internal void AddItemToOrder(string itemname, int amount)
        {
            if(Items.Where(item => item.ThisItem.name == itemname).Count() > 0)
            {
                throw new ArgumentException("This order already contains an entry for that item.");
            }

            Items.Add(new ItemCount(amount, itemname));
        }

        /// <summary>
        /// Creates a new IOrder that will not modify store stocks.
        /// Constructor.
        /// </summary>
        /// <param name="locationName">The name of the store at which the order was placed</param>
        /// <param name="customerName">The name of the customer</param>
        /// <param name="time">The time the order was placed</param>
        /// <param name="Items">List of items in the order</param>
        /// <param name="cost">Total cost of the order when it was placed</param>
        /// <param name="OrderID">A unique ID of the order.</param>
        internal HistoricOrder(string locationName, Name customerName, DateTime time, ICollection<ItemCount> Items, decimal cost, int OrderID)
        {
            OrderLoc = Locations.Instance.GetLocation(locationName);
            Customer = Customers.Instance.GetCustomer(customerName);
            this.Time = time;
            this.Items = Items;
            this.Cost = cost;
            this.ID = OrderID;
        }
    }
}
