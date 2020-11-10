using System;
using System.Collections.Generic;
using System.Diagnostics;
using static MyStore.Store.StoreCatalogue;

namespace MyStore.Store
{
    /// <summary>
    /// Represents a store location.
    /// </summary>
    public class Location
    {
        /// <summary>
        /// Where the store is located. Can also be though of it's name. This is the
        /// primary key when referencing stores.
        /// </summary>
        public String Where { get; }
        //Invintory
        /// <summary>
        /// The amount of various items in stock currently.
        /// </summary>
        private Dictionary<string, ItemCount> Stocks;

        //order history
        /// <summary>
        /// The list of orders so far.
        /// </summary>
        public OrderHistory LocationOrderHistory { get; }


        internal Location(string where)
        {
            this.Where = where;
        }


        //order function
        //must reject if not enough of an item in stock.
        //possibly modify remaining stock
        //place orders, required functionality
        /// <summary>
        /// Part of allowing people to place orders. All orders must be finallized to
        /// Actually place them.
        /// </summary>
        /// <remarks>
        /// NOTE: while amount of items can be changed, customer and location cannot currently be
        /// changed. Instead a new order would have to be created.
        /// </remarks>
        /// <param name="item">The Item object being bought.</param>
        /// <param name="amount">The amount of the item to be ordered. Can be changed later.</param>
        /// <param name="forCustomer">Customer placing the order.</param>
        /// <returns></returns>
        public Order CreateNewOrder(IItem item, int amount, Customer forCustomer)
        {
            List<ItemCount> items = new List<ItemCount>();
            items.Add(new ItemCount(amount, item.name));
            Order neworder = new Order(this, forCustomer, items);
            return neworder;
        }

        /// <summary>
        /// Creates a new order to be edited until done. 
        /// </summary>
        /// <param name="itemName">The human readable name of the first item in the order.</param>
        /// <param name="amount">Amount of the first item in the order.</param>
        /// <param name="forCustomer">Customer placing the order.</param>
        /// <returns>The order object that can be edited until finalized.</returns>
        public Order CreateNewOrder(String itemName, int amount, Customer forCustomer)
        {
            if (StoreCatalogue.Instance.ItemExists(itemName))
            {
                return CreateNewOrder(StoreCatalogue.Instance.GetItem(itemName), amount, forCustomer);

            } else
            {
                throw new ItemNotFoundException("Failed to find item, " + itemName);
            }
        }


        /// <summary>
        /// Add an amount of an item to the Location's invintory.
        /// </summary>
        /// <param name="itemName">The human readable name of the item.</param>
        /// <param name="amount">Amount of the item to add.</param>
        /// <returns>The new Stock Level.</returns>
        public int  AddInventory(string itemName, int amount)
        {
            if(amount < 0)
            {
                throw new ArgumentOutOfRangeException("Must add a positive amount of stock.");
            }

            ItemCount itemStocks;
            if(Stocks.TryGetValue(itemName, out itemStocks))
            {
                int newAmount = amount + itemStocks.Count;
                Stocks[itemName] = new ItemCount(newAmount, itemName);
                return newAmount;
            } else if(StoreCatalogue.Instance.ItemExists(itemName)){
                itemStocks = new ItemCount(amount, itemName);
                return amount;
            } else
            {
                //To be fair, this would already be thrown by the constructor for ItemCount
                throw new ItemNotFoundException("That item doesn't exist yet.");
            }
        }

        /// <summary>
        /// Check if there is enough stock left to subtract the amount from the remaining.
        /// </summary>
        /// <remarks>
        /// If a negative value is passed in, math.abs will be ran on it to compare to the current
        /// level of stocks.
        /// </remarks>
        /// <param name="itemName">The human readable name of the item.</param>
        /// <param name="amount">Amount to be subtracted</param>
        /// <returns></returns>
        public bool CheckIfEnoughStock(string itemName, int amount)
        {
            if(amount < 0)
            {
                amount = Math.Abs(amount);
                Debug.WriteLine("Warning, tried to subtract a negative amount, this has been corrected.");
            }

            ItemCount count;
            if (Stocks.TryGetValue(itemName, out count))
            {
                return count.Count >= amount;
            } else if(amount == 0)
            {
                return true;
            } else
            {
                return false;
            }           
        }

        /// <summary>
        /// Checks to see how much of an item is currently in stock at this location.
        /// </summary>
        /// <remarks>
        /// Note, this doesn't check if the item has been registered with the store catalogue.
        /// </remarks>
        /// <param name="itemName">The Human readable name of the item.</param>
        /// <returns>The amount of Item currently in stock, or zero if that item isn't in stock.</returns>
        public int CheckStock(string itemName)
        {
            ItemCount count;
            if (Stocks.TryGetValue(itemName, out count))
            {
                return count.Count;
            }
            return 0;
        }

        /// <summary>
        /// Remvoes the items from the stock.
        /// </summary>
        /// <remarks>
        /// Assumes the quantity is positive, and is intended to be subtracted.
        /// </remarks>
        /// <param name="ic">The Item-Count pair to be removed.</param>
        internal void RemovePurchasedStock(ItemCount ic)
        {
            if(this.CheckIfEnoughStock(ic.ThisItem.name, ic.Count))
            {
                ItemCount itemStocks;
                if (Stocks.TryGetValue(ic.ThisItem.name, out itemStocks))
                {
                    int newAmount = itemStocks.Count - ic.Count;
                    Stocks[ic.ThisItem.name] = new ItemCount(newAmount, ic.ThisItem.name);
                }
                else
                {
                    throw new ItemNotFoundException($"Error: failed to find {ic.ThisItem.name} in the location's stocks");
                }               
            }
        }
    }
}
