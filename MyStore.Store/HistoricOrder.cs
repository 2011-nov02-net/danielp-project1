using System;
using System.Collections.Generic;
using System.Text;

namespace MyStore.Store
{
    internal class HistoricOrder : IOrder
    {
        public Location OrderLoc { get; }

        public Customer Customer { get; }

        public DateTime Time { get; }

        public ICollection<ItemCount> Items { get; }

        public decimal Cost { get;  }

        internal void EditOrderAmounts(string itemname, int amount)
        {
            Items.Add(new ItemCount(amount, itemname));
        }

        internal HistoricOrder(string locationName, Name customerName, DateTime time, ICollection<ItemCount> Items, decimal cost)
        {
            OrderLoc = Locations.Instance.GetLocation(locationName);
            Customer = Customers.Instance.GetCustomer(customerName);
            this.Time = time;
            this.Items = Items;
            this.Cost = cost;
        }
    }
}
