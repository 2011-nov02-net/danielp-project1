using System;
using System.Collections.Generic;
using System.Linq;
using MyStore.Store;

namespace MyStore.ConsoleView
{
    internal class OrderMenu : IMenu
    {
        private Customer currentCustomer;
        private Location selectedStore;
        private DataModel.IDbRepository Repo;

        public OrderMenu(DataModel.IDbRepository repo, Customer currentCustomer, Location selectedStore)
        {
            Repo = repo;
            this.currentCustomer = currentCustomer;
            this.selectedStore = selectedStore;
        }

        public IMenu DisplayMenu()
        {
            //getting this for logic check here early.
            List<ItemCount> stocks = selectedStore.GetLocationStock().Where(curritem => curritem.Count > 0).ToList();

            if(stocks.Count() <= 0)
            {
                Console.WriteLine($"I'm sorry, {selectedStore.Where} has nothing in stock to buy.");
                Console.WriteLine(" . . . returning to previous menu.");
                return new ViewStoreMenu(Repo, currentCustomer, selectedStore);
            }


            //would you like to see what's in stock? y/n
            Console.WriteLine($"Would you like to see a list of all items in stock at {selectedStore.Where}? y/n");
            int choice = Program.ValidYesNoOption();

            if (choice == 0) //yes
            {
                ViewStock(selectedStore);
            }

            
            List<string> items = new List<string>();
            foreach (ItemCount ic in stocks)
            {
                //add each item to the choices
                items.Add(ic.ThisItem.name.ToLower().Trim());
            }

            int itemindex;          

            do
            {
                Console.Out.WriteLine("\nType the name of the item you wish to buy");
            } while (!Program.ValidOption(Console.ReadLine(), items, out itemindex));
           
            /*
             * At this point, stocks should be a list of all items with at least one in stock, items should be an
             * index aligned list of their names
             * and itemindex should be the index of the item name that the customer types.
             */

            int amountofitem = 0;
            Console.WriteLine($"How many of {items[itemindex]}s would you like to buy?");
            int amount = Program.GetIntegerAmount(max: amountofitem, min: 0);

            Order currentOrder = selectedStore.CreateNewOrder(stocks[itemindex].ThisItem, amountofitem, currentCustomer);

            return new EditOrderMenu(Repo, currentCustomer, selectedStore, currentOrder);
        }


        /// <summary>
        /// checks the current invintory from the model
        /// </summary>
        /// <remarks>
        /// Explictily will not ask the database for any stock changes for simplicity.
        /// </remarks>
        public static void ViewStock(Location thisSelectedStore)
        {
            if (thisSelectedStore.GetLocationStock().Count() <= 0)
            {
                Console.WriteLine("\nThis Location has no items to sell currently.");
                return;
            }

            Console.WriteLine($"{thisSelectedStore.Where} has the following currently in stock:");
            foreach (ItemCount ic in thisSelectedStore.GetLocationStock())
            {
                Console.WriteLine($"{ic.ThisItem.name,20}\t{ic.ThisItem.cost:C}\tWith {ic.Count} in stock.");
            }
            Console.WriteLine("\n");
        }

    }
}