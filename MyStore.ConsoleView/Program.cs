using System;
using MyStore.Store;
using MyStore.Store.Serialization;

namespace MyStore.ConsoleView
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");


            StoreCatalogue.Instance.RegisterItem("Item1", 1);
            StoreCatalogue.Instance.RegisterItem("Item2", 2);
            StoreCatalogue.Instance.RegisterItem("Item3", 3);
            StoreCatalogue.Instance.RegisterItem("Item4", 4);
            StoreCatalogue.Instance.RegisterItem("Item5", 5);


            Locations.Instance.RegisterLocation("Store1");
            Locations.Instance.GetLocation("Store1").AddInventory("Item1", 50);
            Locations.Instance.GetLocation("Store1").AddInventory("Item2", 1);
            Locations.Instance.GetLocation("Store1").AddInventory("Item3", 10);

            Locations.Instance.RegisterLocation("Store2");
            Locations.Instance.GetLocation("Store2").AddInventory("Item4", 50);
            Locations.Instance.GetLocation("Store2").AddInventory("Item5", 60);
            Locations.Instance.GetLocation("Store2").AddInventory("Item2", 25);

            Customer c = Customers.Instance.RegisterCustomer("Daniel", "last", 'm');
            Customers.Instance.RegisterCustomer("Randel", "last", 'n');
            Customers.Instance.RegisterCustomer("Daniel", "last");

            Console.WriteLine("Creating first order");
            Order o = Locations.Instance.GetLocation("Store1").CreateNewOrder("Item1", 10, c);
            o.FinallizeOrder();


            o = Locations.Instance.GetLocation("Store2").CreateNewOrder("Item5", 1, c);
            o.EditOrderAmounts("Item2", 5);
            o.FinallizeOrder();


            XMLWriter writer = new XMLWriter();
            writer.WriteAllData();
        }
    }
}
