using System;
using System.Collections.Generic;
using System.Linq;
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


        public Order(Location l, Customer c, ICollection<ItemCount> items)
        {
            Time = DateTime.UtcNow;
            Customer = c;

            //CHECK LOCATION STOCKS
            bool EnoughStock = true;
            foreach( ItemCount ic in items)
            {
                EnoughStock = EnoughStock && l.CheckIfEnoughStock(ic.ThisItem.name, ic.Count);
            }

            if (EnoughStock)
            {
                Items = items.ToList<ItemCount>().AsReadOnly();
                OrderLoc = l;
            } else
            {
                //TODO: consider making this a new exception.
                throw new NotEnoughStockException($"Not Enough stock at {l.Where}");
            }

        }

        //must check stocks, and if item is already in order.
        void IOrder.AddItemToOrder(string itemname, int amount)
        {
            throw new NotImplementedException();
        }

        //must double check stocks
        //then must add to the order histories of customer and location.
        //also must then change the stocks to reflect the change in stock.
        //POSSIBLE THING TO ADD, Event call somewhere to notify other orders that 
        //stock has changed.
        void IOrder.FinallizeOrder()
        {
            throw new NotImplementedException();
        }
    }
}
