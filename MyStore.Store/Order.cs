using System;
using System.Collections.Generic;
using System.Text;

namespace MyStore.Store
{
    class Order
    {
        Location orderLoc;
        Customer customer;
        DateTime Time;

        //items and amount
        ICollection<Item> items;
    }
}
