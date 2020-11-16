using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using MyStore.Store.Exceptions;

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
        /// <summary>
        /// The time the order was placed.
        /// </summary>
        public DateTime Time { get; private set; }

        private List<ItemCount> _items;

        //items and amount, optionally any price modifyer too for sales
        //must reject unreasonable number of items.
        public ICollection<ItemCount> Items 
        {
            get
            {
                return _items.AsReadOnly();
            } 
            private set
            {
                if(OrderLoc is null)
                {
                    throw new NullReferenceException("This order has no store yet.");
                }

                if (EnoughStockForAllItems())
                {
                    _items = value.ToList<ItemCount>();
                    
                }
                else
                {
                    throw new NotEnoughStockException($"Not Enough stock at {OrderLoc.Where}");
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
            _items = items.ToList<ItemCount>();
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
            List<ItemCount> _items = new List<ItemCount>();
            _items.Add(item);
        }

        //must check stocks, and if item is already in order.
        /// <summary>
        /// Add or change the amount of an item being ordered.
        /// </summary>
        /// <remarks>
        /// Can throw an argument out of range exception if the amount would lead to a negative 
        /// number of itemname being in the order. Can also throw a Not Enough Stock exception
        /// if there is not enough stock to be bought at the current location for the new ammount.
        /// </remarks>
        /// <param name="itemname">The name of the item</param>
        /// <param name="amount">The amount, + or -, to change the quantity of item in the order by.</param>
        public void EditOrderAmounts(string itemname, int amount)
        {
            ItemCount newcount;      
            foreach(ItemCount ic in Items)
            {
                ItemCount oldcount;
                if (ic.ThisItem.name == itemname)
                {
                    oldcount = ic;

                    if(amount + oldcount.Count >= 0 
                        && OrderLoc.CheckIfEnoughStock(itemname, amount + oldcount.Count))
                    {
                        //avoid duplicate entries for the item in the list of items.
                        _items.Remove(oldcount);
                        amount += oldcount.Count;
                    } else
                    {
                        if(amount + oldcount.Count < 0)
                        {
                            throw new ArgumentOutOfRangeException("Error: Would be buying a negative amount of the item.");
                        }else
                        {
                            throw new NotEnoughStockException("Not enough stock to add this many of the item.");
                        }                          
                    }                  
                    break;
                }
            }

            //Make sure this isn't removing an item from the order before adding the new
            //ItemCount to the list of items in the order
            if(amount > 0)
            {
                newcount = new ItemCount(amount, itemname);
                _items.Add(newcount);
            }
        }


        public void DeleteOrder()
        {
            //TODO: reset stocks, delete from Orders.instance
            throw new NotImplementedException();
        }



        //must double check stocks
        //then must add to the order histories of customer and location.
        //also must then change the stocks to reflect the change in stock.
        //POSSIBLE THING TO ADD, Event call somewhere to notify other orders that 
        //stock has changed.
        /// <summary>
        /// MUST be called to place the order and add it to the customer and location's 
        /// order history, and update the location's stocks.
        /// </summary>
        public void FinallizeOrder()
        {
            this.Time = DateTime.UtcNow;


            if (EnoughStockForAllItems())
            {
                //consider surrounding in a try catch, 
                //or something incase this fails.
                //Or, make a mutex for when we change stocks on the Location.
                foreach(ItemCount ic in Items)
                {
                    OrderLoc.RemovePurchasedStock(ic);
                }
                Orders.Instance.AddOrders(this);

            } else
            {
                throw new NotEnoughStockException("Error: No longer enough stock for these items.");
            }
        }


        /// <summary>
        /// Check if theres enough stock at the location for all items in the order.
        /// </summary>
        /// <returns></returns>
        private bool EnoughStockForAllItems()
        {
            //CHECK LOCATION STOCKS
            bool EnoughStock = true;

            //Required Functionality
            //TODO: stop unreasonabley large orders.
            foreach (ItemCount ic in Items)
            {
                EnoughStock = EnoughStock && OrderLoc.CheckIfEnoughStock(ic.ThisItem.name, ic.Count);
            }

            return EnoughStock;
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("OrderLoc", OrderLoc.Where);
            info.AddValue("Customer", Customer.CustomerName, typeof(Name));
            info.AddValue("When", Time);
            info.AddValue("Items", this._items);
        }
    }
}
