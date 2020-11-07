using System;
using static MyStore.Store.StoreCatalogue;

namespace MyStore.Store
{
    public class Location
    {
        String where;
        //Invintory
        Invintory invintory;

        //order history
        OrderHistory orderHistory;

        //order function
        //must reject if not enough of an item in stock.
        public void Order(Item item, Customer forCustomer)
        {
            throw new NotImplementedException();
        }

    }
}
