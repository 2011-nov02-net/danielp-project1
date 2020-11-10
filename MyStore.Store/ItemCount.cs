using System;
using System.Collections.Generic;
using System.Text;


namespace MyStore.Store
{
    //TODO: conisder make this a class
    //then wouldn't have to create a new one to change the amount.
    public struct ItemCount
    {
        public int Count { get; }
        public IItem ThisItem { get; }

        public ItemCount(int count, string itemName)
        {
            Count = count;
            ThisItem = StoreCatalogue.Instance.GetItem(itemName);         
        }
    }
}
