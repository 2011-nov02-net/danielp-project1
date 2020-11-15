using System;
using System.Collections.Generic;
using MyStore.Store;
using MyStore.Store.Serialization;

namespace MyStore.ConsoleView
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to the store!");

            //basically hold the current state of the program
            IMenu CurrentMenu = new StartMenu();

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
        internal static bool ValidOption(string input, List<String> validInput, out int ChoiceIndex)
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
            }

            return WasValid;
        }

        private void Setup()
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

            Customer c = Customers.Instance.RegisterCustomer("Daniel", "last", 'm');
            Customers.Instance.RegisterCustomer("Randel", "last", 'n');
            Customers.Instance.RegisterCustomer("Daniel", "last");

            Console.WriteLine("Creating first order");
            Order o = Locations.Instance.GetLocation("Store1").CreateNewOrder("Item1", 10, c);
            o.FinallizeOrder();


            o = Locations.Instance.GetLocation("Store2").CreateNewOrder("Item5", 1, c);
            o.EditOrderAmounts("Item2", 5);
            o.FinallizeOrder();
        }
    }
}
