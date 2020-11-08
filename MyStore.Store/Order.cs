using System;
using System.Collections.Generic;
using System.Text;

namespace MyStore.Store
{
    public class Order: IOrder
    {
        /*
         * what should happen if a customer decides to change the order location?
         *      should probably maintian items, 
         *      not allow change to another store with not enough stock.
         *      or remove items that aren't in stock there
         */
        public Location OrderLoc { get; }
        public Customer Customer { get; }
        public DateTime Time { get; }

        //items and amount, optionally any price modifyer too for sales
        //must reject unreasonable number of items.
        public ICollection<ItemCount> Items { get; }
    }
}
