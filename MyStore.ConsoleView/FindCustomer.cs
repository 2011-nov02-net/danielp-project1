using System;
using System.Collections.Generic;
using MyStore.Store;
using MyStore.Store.Exceptions;

namespace MyStore.ConsoleView
{
    internal class FindCustomer : IMenu
    {
        private DataModel.IDbRepository Repo;

        public FindCustomer(DataModel.IDbRepository repo)
        {
            Repo = repo;
        }

        public IMenu DisplayMenu()
        {
            //ASK And possibly display list of all customers
            Console.WriteLine("Would you like to see a list of all customers? y/n");
            int choice = Program.ValidYesNoOption();

            if (choice == 0) //yes
            {
                DisplayCustomers();
            }


            bool FoundCustomer = false;
            Customer Current = null;
            do
            {
                FoundCustomer = false;

                //get a name from the user
                Name fullname = GetName();

                //check if customer exists in the model, if so found customer = true
                try
                {
                    Current = Customers.Instance.GetCustomer(fullname);
                }
                catch (CustomerNotFoundException e)
                {
                    Console.Error.WriteLine(e.Message);
                    Current = Repo.GetCustomerByName(fullname);
                }

                if (Current != null)
                {
                    FoundCustomer = true;
                }
                else
                {
                    //customer could not be found:
                    //ask if they want to make a new customer instead.
                    Console.WriteLine("\nWhat would you like to do?");
                    Console.WriteLine("{F}ind - continue looking for a customer.");
                    Console.WriteLine("{V}iew - view a list of existing customers.");
                    Console.WriteLine("{N}ew - Create a new customer with the name " + fullname.ToString());


                    choice = -1;
                    while (!Program.ValidOption(Console.ReadLine(),
                        new List<string> { "f", "find", "v", "view", "n", "new" },
                        out choice))
                    {

                    }

                    switch (choice)
                    {
                        case 0:
                        case 1:
                            //do nothing, get new name 
                            break;
                        case 2:
                        case 3:
                            //show a list of customers.
                            DisplayCustomers();
                            break;
                        case 4:
                        case 5:
                            return new CreateCustomer(Repo).DisplayMenu(fullname);
                            break;
                        default:
                            //error state
                            Console.Error.WriteLine("Error, unexpected input in FindCustomer.");
                            return this;
                    }
                }
            }
            while (!FoundCustomer);

            return new LoggedInMenu(Repo, Current);
        }


        public static Name GetName()
        {
            //Get first name
            Console.WriteLine("What is your/the desired customer's first name?");
            string firstName = Console.ReadLine().Trim();

            if(firstName.Length >= 20)
            {
                firstName = firstName.Substring(0, 20);
            }

            //ask if middle inital
            Console.WriteLine("Does you/the desired customer have a middle name? y/n");
            int hasMiddle = Program.ValidYesNoOption();

            char? middleInitial = null;
            if (hasMiddle == 0)
            {
                //get middle name.
                Console.WriteLine("What is your/the desired customer's first name?");
                middleInitial = Console.ReadLine().Trim()[0];
            }

            //get last name
            Console.WriteLine("What is your/the desired customer's last name?");
            string lastName = Console.ReadLine().Trim();

            if (lastName.Length >= 20)
            {
                lastName = lastName.Substring(0, 20);
            }


            return new Name(firstName, lastName, middleInitial);
        }


        public void DisplayCustomers()
        {
            IEnumerable<Store.Customer> customers = Repo.GetCustomers();

            foreach (Store.Customer c in customers)
            {
                Console.WriteLine($"{c}");
            }

            Console.WriteLine("\n"); //write a few lines
        }
    }
}