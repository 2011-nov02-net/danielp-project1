using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.EntityFrameworkCore;
using MyStore.DataModel;
using MyStore.Store;
using MyStore.Store.Serialization;

namespace MyStore.ConsoleView
{
    class Program
    {
        static void Main(string[] args)
        {
            DbContextOptions<Project0DBContext> dbContextOptions = SetupContextOptions();

            if(dbContextOptions == null)
            {
                Console.WriteLine("Exiting Program . . .");
                return;
            }

            IDbRepository repo = new DBRepository(dbContextOptions);
            Console.WriteLine("Welcome to the store!");

            //basically hold the current state of the program
            IMenu CurrentMenu = new StartMenu(repo);

            while( CurrentMenu != null)
            {
                CurrentMenu = CurrentMenu.DisplayMenu();
            }
        }


        /// <summary>
        /// Checks if the input string from a user was valid.
        /// </summary>
        /// <param name="input">The user's input string. Will be trimmed and set to lowercase.</param>
        /// <param name="validInput">The list of valid input strings.</param>
        /// <param name="ChoiceIndex">The index of the user's choice in the validInput array, or -1 if it was not in the array.</param>
        /// <returns></returns>
        internal static bool ValidOption(string input, List<string> validInput, out int ChoiceIndex)
        {
            //normalize input
            input = input.Trim().ToLower();
            ChoiceIndex = -1;
            bool WasValid = false;

            for(int i = 0; i < validInput.Count && !WasValid; i++)
            {
                if(validInput[i] == input)
                {
                    ChoiceIndex = i;
                    WasValid = true;
                }
            }

            if (!WasValid)
            {
                Console.WriteLine($"\"{input}\" Was not recognized as valid input.");

                Console.Write("Please choose from:");

                foreach(string option in validInput)
                {
                    Console.Write($" \"{option}\",");
                }

                Console.WriteLine();
            }

            return WasValid;
        }


        /// <summary>
        /// Reads and validates a response to a yes or no question already asked.
        /// </summary>
        /// <returns> 0 for yes, 1 for no. </returns>
        public static int ValidYesNoOption()
        {
            int choice;
            //wait until valid response.
            while (!Program.ValidOption(Console.ReadLine(), new List<string> { "y", "n" }, out choice))
            {

            }
            
            return choice;
        }


        private static DbContextOptions<Project0DBContext> SetupContextOptions()
        {
            DbContextOptionsBuilder<Project0DBContext> optionsBuilder = new DbContextOptionsBuilder<Project0DBContext>();
            string fileloc = "./../../../../MyStore.dataModel/ConnectionString.txt";
            if (!File.Exists(fileloc))
            {
                Console.WriteLine($"Error: Expected a file called \"ConnectionString.txt\" at {fileloc} holding only the database connection string.");
                return null;
            }

            string connectionStr = File.ReadAllText(fileloc);

            optionsBuilder.UseSqlServer(connectionStr);
            return optionsBuilder.Options;
        }

        private static void Setup()
        {

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

            Store.Customer c = Customers.Instance.RegisterCustomer("Daniel", "last", 'm');
            Customers.Instance.RegisterCustomer("Randel", "last", 'n');
            Customers.Instance.RegisterCustomer("Daniel", "last");

            Console.WriteLine("Creating first order");
            Store.Order o = Locations.Instance.GetLocation("Store1").CreateNewOrder("Item1", 10, c);
            o.FinallizeOrder();


            o = Locations.Instance.GetLocation("Store2").CreateNewOrder("Item5", 1, c);
            o.EditOrderAmounts("Item2", 5);
            o.FinallizeOrder();
        }
    }
}
