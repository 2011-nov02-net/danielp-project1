using System;
using static MyStore.Store.StoreCatalogue;

namespace MyStore.Store
{
    public class Location
    {
        public String Where { get; }
        //Invintory
        private Invintory LocationInvintory;

        //order history
        private OrderHistory LocationOrderHistory;

        //order function
        //must reject if not enough of an item in stock.
        //TODO: place orders, required functionality
        public void PlaceOrder(IItem item, Customer forCustomer)
        {
            throw new NotImplementedException();
        }


        public Location(string where)
        {
            throw new NotImplementedException();
        }

        public void AddInvintory(string itemName, int amount)
        {
            throw new NotImplementedException();
        }


        public int CheckStock(string itemName, int amount)
        {
            throw new NotImplementedException();
        }
    }
}
