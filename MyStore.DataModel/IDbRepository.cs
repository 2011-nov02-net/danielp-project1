using System;
using System.Collections.Generic;

namespace MyStore.DataModel
{
    //https://www.learnentityframeworkcore.com/dbcontext/modifying-data
    //https://codewithshadman.com/repository-pattern-csharp/
    //https://github.com/2011-nov02-net/trainer-code/wiki/Project-0-requirements
    public interface IDbRepository
    {
        //gets data, converts the data to the model's form.

        //console handles giving this the changed object, this handles sending that to db entities
        //probably with a save function

        Store.Customer GetCustomerByName(Store.Name name);

        IEnumerable<Store.Customer> GetCustomers();


        void CreateCustomer(Store.Customer customer);

        IEnumerable<Store.IOrder> GetOrderHistory(Store.Customer c);

        IEnumerable<Store.IOrder> GetOrderHistory(Store.Location l);

        IEnumerable<Store.ItemCount> GetStoreStocks(Store.Location l);


        void PlaceOrder(Store.Order o);

        void UpdateOrder(Store.Order o);

    }
}
