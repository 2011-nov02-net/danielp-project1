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
        public ICollection<ItemCount> Items 
        {
            get
            {
                //todo: make this into a read only collection when sent back.
                return Items;
            } 
            private set
            {
                if(OrderLoc is null)
                {
                    throw new NullReferenceException("This order has no store yet.");
                }

                //CHECK LOCATION STOCKS
                bool EnoughStock = true;

                foreach (ItemCount ic in value)
                {
                    EnoughStock = EnoughStock && OrderLoc.CheckIfEnoughStock(ic.ThisItem.name, ic.Count);
                }

                if (EnoughStock)
                {
                    Items = value.ToList<ItemCount>().AsReadOnly();
                    
                }
                else
                {
                    throw new NotEnoughStockException($"Not Enough stock at {l.Where}");
                }
            }
        }


        /// <summary>
        /// Create a new order of multiple items.
        /// </summary>
        /// <remarks>
        /// Can throw a NotEnoughItemsException if the store does not have enough of the items.
        /// </remarks>
        /// <param name="l">The Location the order is being placed at.</param>
        /// <param name="c">The customer placing the order.</param>
        /// <param name="items">A collection of item counts to be ordered.</param>
        public Order(Location l, Customer c, ICollection<ItemCount> items)
        {
            Time = DateTime.UtcNow;
            Customer = c;
            OrderLoc = l;
            this.Items = items;
        }

        /// <summary>
        /// Creates a new order for one item.
        /// </summary>
        /// <param name="l">The Location the order is being placed at.</param>
        /// <param name="c">The customer placing the order.</param>
        /// <param name="item">The item and amount being ordered.</param>
        public Order(Location l, Customer c, ItemCount item)
        {
            Time = DateTime.UtcNow;
            Customer = c;
            OrderLoc = l;
            List<ItemCount> items = new List<ItemCount>();
            items.Add(item);
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
