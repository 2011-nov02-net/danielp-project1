using System;
using System.Collections.Generic;
using System.Text;


namespace MyStore.Store
{
    /// <summary>
    /// A struct to pair items with amounts of that item
    /// </summary>
    public struct ItemCount
    {
        public int Count { get; }
        public IItem ThisItem { get; }

        public ItemCount(int count, string itemName)
        {
            Count = count;
            ThisItem = StoreCatalogue.Instance.GetItem(itemName);         
        }


        public ItemCount(int count, IItem item)
        {
            Count = count;
            ThisItem = item;
        }

    }
}
