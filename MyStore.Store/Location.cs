using System;
using System.Collections.Generic;
using System.Diagnostics;
using static MyStore.Store.StoreCatalogue;

namespace MyStore.Store
{
    public class Location
    {
        public String Where { get; }
        //Invintory
        private Dictionary<string, ItemCount> Stocks;

        //order history
        public OrderHistory LocationOrderHistory { get; }


        internal Location(string where)
        {
            this.Where = where;
        }


        //order function
        //must reject if not enough of an item in stock.
        //possibly modify remaining stock
        //TODO: place orders, required functionality
        public Order CreateNewOrder(IItem item, int amount, Customer forCustomer)
        {
            List<ItemCount> items = new List<ItemCount>();
            items.Add(new ItemCount(amount, item.name));
            Order neworder = new Order(this, forCustomer, items);
            return neworder;
        }


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


        public int CheckStock(string itemName)
        {
            ItemCount count;
            if (Stocks.TryGetValue(itemName, out count))
            {
                return count.Count;
            }
            return 0;
        }
    }
}
