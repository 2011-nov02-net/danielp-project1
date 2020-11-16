using System;
using System.Collections.Generic;
using MyStore.DataModel;

namespace MyStore.ConsoleView
{
    internal class StartMenu : IMenu
    {
        //                                                   0     1      2     3     4    5
        private List<string> ValidInputs = new List<string>{"q", "quit", "n", "new", "f", "find" };
        private IDbRepository Repo;

        public StartMenu(IDbRepository repo)
        {
            Repo = repo;
        }

        public IMenu DisplayMenu()
        {
            int userchoice; 

            do
            {
                Console.WriteLine("Please choose from the following options:");
                Console.WriteLine("{Q}uit - Exit the program.");
                Console.WriteLine("{N}ew - Create a new customer to use.");
                Console.WriteLine("{F}ind - Find an existing customer to use.");
            }
            while (!Program.ValidOption(Console.ReadLine(), ValidInputs, out userchoice));


            switch (userchoice)
            {
                case 0:
                case 1:
                    Console.WriteLine("Exiting Program.");
                    return null;
                    break;
                case 2:
                case 3:
                    return new CreateCustomer(Repo);
                    break;
                case 4:
                case 5:
                    return new FindCustomer(Repo);
                    break;
                default:
                    //Some error has happened, just go back and do this menu over again.
                    return this;
                    break;
            }
        }
    }
}