using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace MyStore.Store
{
    public class OrderHistory : ISerializable
    {
        private List<IOrder> _orderHistory;
        //order history for a thing
        public IReadOnlyCollection<IOrder> orderhistory 
        { 
            get
            {
                return _orderHistory.AsReadOnly();
            } 
        }

        /// <summary>
        /// Assumes
        /// </summary>
        /// <param name="o"></param>
        internal void AddOrder(IOrder o)
        {
            _orderHistory.Add(o);
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            ((ISerializable)_orderHistory).GetObjectData(info, context);
        }

        public OrderHistory()
        {
            _orderHistory = new List<IOrder>();
        }
    }
}
