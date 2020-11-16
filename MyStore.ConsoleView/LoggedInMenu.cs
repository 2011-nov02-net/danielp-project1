using System;
using System.Collections.Generic;
using MyStore.Store;

namespace MyStore.ConsoleView
{
    internal class LoggedInMenu : IMenu
    {
        private Customer LoggedInCustomer;
        private DataModel.IDbRepository Repo;

        private List<string> ValidInputs = new List<string> { "q", "quit", "l", "log out", "v", "view orders", "f", "find store" };

        public LoggedInMenu(DataModel.IDbRepository repo, Customer current)
        {
            this.LoggedInCustomer = current;
            Repo = repo;
        }

        public IMenu DisplayMenu()
        {
            int userchoice;


            do
            {
                Console.WriteLine("Please choose from the following options:");
                Console.WriteLine("{Q}uit - Exit the program.");
                Console.WriteLine("{L}og Out - Change to a different customer.");
                Console.WriteLine("{V}iew orders - See the current customer's order history.");
                Console.WriteLine("{F}ind store - select a store to interact with and order from.");
            }
            while (!Program.ValidOption(Console.ReadLine(), ValidInputs, out userchoice));

            switch (userchoice)
            {
                //Quit
                case 0:
                case 1:
                    Console.WriteLine("Exiting Program.");
                    return null;
                    break;
                //log out
                case 2:
                case 3:
                    return new StartMenu(Repo);
                    break;
                //view customer orders
                case 4:
                case 5:
                    DisplayCustomerOrders();
                    return this;
                    break;
                //find store
                case 6:
                case 7:
                    return new SelectStoreMenu(Repo, LoggedInCustomer);
                    break;
                //Some error has happened, just go back and do this menu over again.
                default:
                    return this;
                    break;
            }
        }

        //TODO: display customer order history
        // asks model for customer's orders, prints them, then returns
        //DisplayMenu should return self for this
        private void DisplayCustomerOrders()
        {
            throw new System.NotImplementedException();
        }
    }
}