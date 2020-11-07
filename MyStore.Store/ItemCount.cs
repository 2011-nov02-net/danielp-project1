using System;
using System.Collections.Generic;
using System.Text;


namespace MyStore.Store
{
    public struct ItemCount
    {
        public int Count { get; }
        public StoreCatalogue.Item ThisItem { get; }

        public ItemCount(int count, string itemName)
        {
            ThisItem = StoreCatalogue.Instance.GetItem(itemName);
            Count = count;
        }
    }
}
