using System;
using System.Collections.Generic;
using MyStore.Store;

namespace MyStore.ConsoleView
{
    internal class SelectStoreMenu : IMenu
    {
        private DataModel.IDbRepository Repo;
        private Customer loggedInCustomer;


        public SelectStoreMenu(DataModel.IDbRepository repo, Customer loggedInCustomer)
        {
            Repo = repo;
            this.loggedInCustomer = loggedInCustomer;
        }


        //The process for selecting a store 
        public IMenu DisplayMenu()
        {
            Location selectedStore = null;

            //ASK And possibly display list of all customers
            Console.WriteLine("Would you like to see a list of all stores? y/n");
            int choice = Program.ValidYesNoOption();

            if (choice == 0) //yes
            {
                DisplayStores();
            }

            bool storeFound = false;
            do
            {
                storeFound = false;

                //get store name
                Console.WriteLine("What is the name of the store you wish to interact with?");
                string storeName = Console.ReadLine();


                //check if it's in db or the modle
                selectedStore = null;
                // limit um 100 characters
                try
                {
                    selectedStore = Locations.Instance.GetLocation(storeName);
                }
                catch (ArgumentException e)
                {
                    Console.Error.WriteLine(e.Message);
                    selectedStore = Repo.GetLocation(storeName);
                }

                if (selectedStore != null)
                {
                    storeFound = true;
                } else
                {
                    //ask what to do next, since user didn't type a real store name
                    Console.WriteLine("\nWhat would you like to do?");
                    Console.WriteLine("{F}ind - continue looking for a store.");
                    Console.WriteLine("{V}iew - view a list of all stores.");

                    //get valid input
                    choice = -1;
                    while (!Program.ValidOption(Console.ReadLine(),
                        new List<string> { "f", "find", "v", "view" },
                        out choice))
                    {

                    }

                    //interpret input
                    switch (choice)
                    {
                        case 0:
                        case 1:
                            //do nothing, get new name 
                            break;
                        case 2:
                        case 3:
                            //show a list of customers.
                            DisplayStores();
                            break;
                        default:
                            //error state
                            Console.Error.WriteLine("Error, unexpected input in FindCustomer.");
                            return this;
                    }
                }
            }
            while (!storeFound);

            return new ViewStoreMenu(Repo, this.loggedInCustomer, selectedStore);
        }

        private void DisplayStores()
        {
            IEnumerable<Store.Location> locations = Repo.GetLocations();
            
            foreach(Location l in locations)
            {
                Console.Out.WriteLine(l.LocationName);
            }
        }
    }
}