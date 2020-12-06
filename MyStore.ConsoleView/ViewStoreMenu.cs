using System;
using System.Collections.Generic;
using MyStore.Store;
using System.Linq;

namespace MyStore.ConsoleView
{
    internal class ViewStoreMenu : IMenu
    {
        private Store.Customer Customer;
        private Location selectedStore;
        private DataModel.IDbRepository Repo;

        public ViewStoreMenu(DataModel.IDbRepository repo, Customer currentCustomer, Location selectedStore)
        {
            Repo = repo;
            Customer = currentCustomer;
            this.selectedStore = selectedStore;
        }

        public IMenu DisplayMenu()
        {
            int userchoice;
            do
            {
                Console.WriteLine("Please choose from the following options:");
                Console.WriteLine("{B}ack - Go back to the logged in menu.");
                Console.WriteLine("{O}rder - Place an order for an item.");
                Console.WriteLine("{V}iew orders - See the store's order history.");
                Console.WriteLine("{S}tocks - View the store's current stocks.");
            }
            while (!Program.ValidOption(Console.ReadLine(), 
                                        new List<string>{"b",  "back", "o", "order", "v", "view", "s", "stocks" },
                                        out userchoice));


            switch (userchoice)
            {
                //back
                case 0:
                case 1:                 
                    return new LoggedInMenu(Repo, Customer);
                    break;
                //order
                case 2:
                case 3:
                    //make sure the model's stock is in synch with the db 
                    //before disconnecting for a while. 
                    //will not ask db while creating order, will only try at the end.
                    ReLoadStoreInvintory();
                    return new OrderMenu(Repo, Customer, selectedStore);
                    break;
                //view store orders
                case 4:
                case 5:
                    this.ViewStoreOrderHistory();
                    return this;
                    break;
                //view store stocks
                case 6:
                case 7:
                    ViewStock();
                    return this;
                    break;
                //Some error has happened, just go back and do this menu over again.
                default:
                    ViewStoreOrderHistory();
                    return this;
                    break;
            }         
        }


        /// <summary>
        /// asks the repo for the stores invintory, and overwrites the current invintory.
        /// </summary>
        private void ReLoadStoreInvintory()
        {
            Repo.UpdateAndOverwriteStoreStocks(selectedStore);
        }

        //Implement view stock
        private void ViewStock()
        {
            //I think the store's stocks should have been pulled from the DB with the store
            ReLoadStoreInvintory();
            if (selectedStore.GetAllStock().Count() <= 0)
            {
                Console.WriteLine("\nThis Location has no items to sell currently.");
                return;
            }

            Console.WriteLine($"{selectedStore.Where} has the following currently in stock:");
            foreach(ItemCount ic in selectedStore.GetAllStock())
            {
                Console.WriteLine($"{ic.ThisItem.name, 20}\t{ic.ThisItem.cost:C}\tWith {ic.Count} in stock.");
            }
            Console.WriteLine("\n");
        }

        //implement view store order history
        private void ViewStoreOrderHistory()
        {
            Console.WriteLine();

            IEnumerable<Store.IOrder> orders = Repo.GetOrderHistory(selectedStore);

            if (orders.Count() <= 0)
            {
                Console.WriteLine("This store has no orders\n");
                return;
            }

            foreach (IOrder o in orders)
            {
                Console.WriteLine($"On {o.Time}, from {o.OrderLoc.Where} by {o.Customer.CustomerName}");

                foreach (ItemCount ic in o.Items)
                {
                    Console.WriteLine($"\t{ic.ThisItem.name,20}\tx{ic.Count}\t{ic.ThisItem.cost:C}");
                }
                Console.WriteLine($"\t{"Order",20}\tTotal:\t{o.Cost:C}\n");
            }           
        }
    }
}