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
        /// <summary>
        /// How much of an item is there
        /// </summary>
        public int Count { get; }

        /// <summary>
        /// the item this is counting.
        /// </summary>
        public IItem ThisItem { get; }

        /// <summary>
        /// simple constructor for ItemCount
        /// </summary>
        /// <param name="count">Amount of the item</param>
        /// <param name="itemName">Name of the item</param>
        public ItemCount(int count, string itemName)
        {
            Count = count;
            ThisItem = StoreCatalogue.Instance.GetItem(itemName);         
        }

        /// <summary>
        /// simple constructor for ItemCount
        /// </summary>
        /// <param name="count">Amount of the item</param>
        /// <param name="item">The item being added</param>
        public ItemCount(int count, IItem item)
        {
            Count = count;
            ThisItem = item;
        }

    }
}
