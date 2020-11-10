using System;
using System.Collections.Generic;
using System.Text;

namespace MyStore.Store
{
    public class OrderHistory
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

        public OrderHistory()
        {
            _orderHistory = new List<IOrder>();
        }
    }
}
