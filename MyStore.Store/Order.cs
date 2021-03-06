﻿using System;
using System.Collections.Generic;
using System.Linq;
using MyStore.Store.Exceptions;

namespace MyStore.Store
{
    /// <summary>
    /// An order that a customer is working on making, or that has been placed.
    /// </summary>
    public class Order: IOrder 
    {
        /// <summary>
        /// The location an order was placed at
        /// </summary>
        public Location OrderLoc { get; }

        /// <summary>
        /// The customer who placed the order
        /// </summary>
        public Customer Customer { get; }

        /// <summary>
        /// The time the order was placed.
        /// </summary>
        public DateTime Time { get; private set; }
       
        /// <summary>
        /// The items in the order
        /// </summary>
        private List<ItemCount> _items = new List<ItemCount>();

        /// <summary>
        /// The total cost of the order. 
        /// </summary>
        /// <remarks>
        /// while this is equal to -1, the value is calculated dynamically,
        /// after the order is finalized, this get's replaced with the total
        /// at time of purchase incase prices change.
        /// </remarks>
        private decimal _orderCost = -1m;

        /// <summary>
        /// The total cost of the order
        /// </summary>
        public decimal Cost
        {
            get
            {
                if(_orderCost < 0)
                {
                    decimal cost = 0;
                    foreach(ItemCount ic in _items)
                    {
                        cost += ic.Count * ic.ThisItem.cost;
                    }

                    return cost;
                } else
                {
                    return _orderCost;
                }
            }
        }

        //items and amount, optionally any price modifyer too for sales
        //must reject unreasonable number of items.
        /// <summary>
        /// The items in the order.
        /// </summary>
        public ICollection<ItemCount> Items 
        {
            get
            {
                return _items.AsReadOnly();
            } 
           
        }

        private int _Id;

        /// <summary>
        /// The ID that uniquely identifies this order
        /// </summary>
        public int ID
        {
            get
            {
                return _Id;
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
            //Id assigned in FinalizeOrder

            if (!this.EnoughStockForAllItems())
            {
                throw new NotEnoughStockException("Not enough of the items @ location for this order.");
            }
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
            //Id assigned in FinalizeOrder
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
        /// <exception cref="NotEnoughStockException">If there's not enough stock.</exception>
        /// <exception cref="ArgumentOutOfRangeException">If there would be a negative amount in the order</exception>
        /// <param name="itemname">The name of the item</param>
        /// <param name="deltaAmount">The amount, + or -, to change the quantity of item in the order by.</param>
        public void EditOrderAmounts(string itemname, int deltaAmount)
        {
            ItemCount newcount;
            ItemCount oldcount;
            //find the old count for the item
            foreach (ItemCount ic in Items)
            {               
                if (ic.ThisItem.name == itemname)
                {
                    oldcount = ic;

                    if(deltaAmount + oldcount.Count >= 0 
                        && OrderLoc.CheckIfEnoughStock(itemname, deltaAmount + oldcount.Count))
                    {
                        //avoid duplicate entries for the item in the list of items.
                        _items.Remove(oldcount);

                        //Make sure this isn't removing an item from the order before adding the new
                        //ItemCount to the list of items in the order
                        if (deltaAmount > 0)
                        {
                            newcount = new ItemCount(oldcount.Count + deltaAmount, oldcount.ThisItem.name);
                            _items.Add(newcount);
                        }

                    } else
                    {
                        if(deltaAmount + oldcount.Count < 0)
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
        }

        /// <summary>
        /// Remove an item from the order
        /// </summary>
        /// <param name="itemName">The name of the item to remove.</param>
        public void RemoveItem(string itemName)
        {
            foreach(ItemCount ic in _items)
            {
                if(ic.ThisItem.name == itemName)
                {
                    _items.Remove(ic);
                    break;
                }
            }
        }


        /// <summary>
        /// Add an item to the order.
        /// </summary>
        /// <exception cref="NotEnoughStockException">If there's not enough stock</exception>
        /// <param name="item">The item to be added</param>
        /// <param name="amount">The amount of the item to be added</param>
        public void AddItem(IItem item, int amount)
        {
            if(OrderLoc.CheckIfEnoughStock(item.name, amount)){
                this._items.Add(new ItemCount(amount, item.name));
            } else
            {
                throw new NotEnoughStockException("not enough stock to buy " + item.name);
            }
        }

        /// <summary>
        /// Add an item to the order by name
        /// </summary>
        /// <param name="itemname">Adds the item to the order</param>
        /// <param name="amount">Adds this much of the item to the order</param>
        public void AddItem(string itemname, int amount)
        {
            AddItem(StoreCatalogue.Instance.GetItem(itemname), amount);
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
        public void FinallizeOrder(int finalID = -999)
        {
            this.Time = DateTime.UtcNow;
            _orderCost = this.Cost;


            if (EnoughStockForAllItems())
            {
                //consider surrounding in a try catch, 
                //or something incase this fails.
                //Or, make a mutex for when we change stocks on the Location.
                foreach(ItemCount ic in Items)
                {
                    OrderLoc.RemovePurchasedStock(ic);
                }

                //assign final ID
                if(finalID == -999)
                {
                    _Id = Orders.Instance.GetAllOrders().Max(order => order.ID) + 1;
                } else
                {
                    _Id = finalID;
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
        /// <returns>True if there's enough stock for the items in the order at the location.</returns>
        private bool EnoughStockForAllItems()
        {
            //CHECK LOCATION STOCKS
            bool EnoughStock = true;

            //Required Functionality
            foreach (ItemCount ic in Items)
            {
                EnoughStock = EnoughStock && OrderLoc.CheckIfEnoughStock(ic.ThisItem.name, ic.Count);
            }

            return EnoughStock;
        }
    }
}
