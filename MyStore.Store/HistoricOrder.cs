using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace MyStore.Store
{
    internal class HistoricOrder : IOrder
    {
        public Location OrderLoc { get; }

        public Customer Customer { get; }

        public DateTime Time { get; }

        public ICollection<ItemCount> Items { get; }

        internal void EditOrderAmounts(string itemname, int amount)
        {
            Items.Add(new ItemCount(amount, itemname));
        }

        internal HistoricOrder(string locationName, Name customerName, DateTime time, ICollection<ItemCount> Items)
        {
            OrderLoc = Locations.Instance.GetLocation(locationName);
            Customer = Customers.Instance.GetCustomer(customerName);
            this.Time = time;
            this.Items = Items;
        }

        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("OrderLoc", OrderLoc.Where);
            info.AddValue("Customer", Customer.CustomerName, typeof(Name));
            info.AddValue("When", Time);
            info.AddValue("Items", Items);
        }
    }
}
