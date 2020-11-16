using System;
using MyStore.Store;
using MyStore.Store.Exceptions;

namespace MyStore.ConsoleView
{
    internal class CreateCustomer : IMenu
    {
        private DataModel.IDbRepository Repo;

        public CreateCustomer(DataModel.IDbRepository repo)
        {
            Repo = repo;
        }

        public IMenu DisplayMenu()
        {
            Customer Current = null;
            bool gotValidName = false;
            Name customerName;


            do
            {
                customerName = FindCustomer.GetName();
                try
                {
                    Current = Customers.Instance.RegisterCustomer(Current);
                    Repo.CreateCustomer(Current);
                    gotValidName = true;

                }
                catch (CustomerAlreadyExistsException e)
                {
                    gotValidName = false;
                    Current = null;
                    Console.WriteLine(e.Message);
                    Console.WriteLine("Names must be unique, that one isn't. Try again please.");
                }

            } while (!gotValidName);

            return new LoggedInMenu(Repo, Current);
        }

        //ASSUMES THE NAME HAS BEEN CHECKED, by FindCustomer.DisplayMenu
        internal IMenu DisplayMenu(Name UnusedName)
        {
            Customer Current = Customers.Instance.RegisterCustomer(UnusedName);
            Repo.CreateCustomer(Current);
            
            //if something has gone wrong, just go to the normal create customer flow.
            if(Current == null)
            {
                return this;
            }

            return new LoggedInMenu(Repo, Current);
        }

    }
}