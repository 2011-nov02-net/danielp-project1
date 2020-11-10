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

        public void EditOrderAmounts(string itemname, int amount)
        {
            Items.Add(new ItemCount(amount, itemname));
        }

        internal HistoricOrder(string locationName, Name customerName, DateTime time)
        {
            OrderLoc = Locations.Instance.GetLocation(locationName);
            Customer = Customers.Instance.GetCustomer(customerName);
            this.Time = time;
            Items = new List<ItemCount>();
        }

        //Will NOT change store's stock
        //WILL add to order histories.
        public void FinallizeOrder()
        {
            Customer.OrderHistory.AddOrder(this);
            OrderLoc.LocationOrderHistory.AddOrder(this);
        }
    }
}
