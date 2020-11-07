using System;
using System.Collections.Generic;
using System.Text;

namespace MyStore.Store
{
    class Order
    {
        /*
         * what should happen if a customer decides to change the order location?
         *      should probably maintian items, 
         *      not allow change to another store with not enough stock.
         *      or remove items that aren't in stock there
         */
        Location orderLoc;
        Customer customer;
        DateTime Time;

        //items and amount, optionally any price modifyer too for sales
        //must reject unreasonable number of items.
        ICollection<ItemCount> items;
    }
}
