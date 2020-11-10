using System;
using System.Collections.Generic;
using System.Text;

namespace MyStore.Store
{
    internal class HistoricOrder : IOrder
    {
        public Location OrderLoc => throw new NotImplementedException();

        public Customer Customer => throw new NotImplementedException();

        public DateTime Time => throw new NotImplementedException();

        public ICollection<ItemCount> Items => throw new NotImplementedException();

        public void EditOrderAmounts(string itemname, int amount)
        {
            throw new NotImplementedException();
        }

        //Will NOT change store's stock
        //WILL add to order histories.
        public void FinallizeOrder()
        {
            throw new NotImplementedException();
        }
    }
}
