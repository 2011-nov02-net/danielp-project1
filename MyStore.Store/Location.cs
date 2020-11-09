using System;
using System.Collections.Generic;
using static MyStore.Store.StoreCatalogue;

namespace MyStore.Store
{
    public class Location
    {
        public String Where { get; }
        //Invintory
        private Dictionary<string, ItemCount> Stocks;

        //order history
        private OrderHistory LocationOrderHistory;





        //order function
        //must reject if not enough of an item in stock.
        //TODO: place orders, required functionality
        public void PlaceOrder(IItem item, Customer forCustomer)
        {
            throw new NotImplementedException();
        }


        internal Location(string where)
        {
            this.Where = where;
        }

        public void AddInvintory(string itemName, int amount)
        {
            throw new NotImplementedException();
        }


        public int CheckStock(string itemName, int amount)
        {
            throw new NotImplementedException();
        }


        public int CheckStock(string itemName)
        {
            ItemCount count;
            if (Stocks.TryGetValue(itemName, out count))
            {
                int basestock = count.Count;

                return basestock;
            }
            return 0;
        }
    }
}
