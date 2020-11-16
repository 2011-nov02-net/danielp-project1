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
            //TODO: ASK And possibly display list of all customers


            bool FoundCustomer = false;
            Customer Current = null;
            do
            {
                //get a name from the user
                Name fullname = GetName();

                //check if customer exists in the model, if so found customer = true
                try
                {
                    Current = Customers.Instance.GetCustomer(fullname);
                } catch (CustomerNotFoundException e) {
                    Current = Repo.GetCustomerByName(fullname);
                }

                if(Current != null)
                {
                    FoundCustomer = true;
                } else
                {
                    //customer could not be found:
                    //ask if they want to make a new customer instead.
                    Console.WriteLine("Customer not found, would you like to make a new customer?");
                    int keepLooking = Program.ValidYesNoOption();

                    if(keepLooking == 0)
                    {
                        return new CreateCustomer(Repo);
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


            return new Name(firstName, lastName, middleInitial);
        }
    }
}