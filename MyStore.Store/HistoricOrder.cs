using System;
using System.Collections.Generic;
using System.Text;

namespace MyStore.Store
{
    internal class HistoricOrder : IOrder
    {
        public Location OrderLoc { get => throw new NotImplementedException();}

        public Customer Customer => throw new NotImplementedException();

        public DateTime Time => throw new NotImplementedException();

        public ICollection<ItemCount> Items => throw new NotImplementedException();
    }
}
