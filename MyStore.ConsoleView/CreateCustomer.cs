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
                    Store.Location defloc = null;
                    Current = Customers.Instance.RegisterCustomer(customerName, defloc);
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
            Location s = null;
            Customer Current = Store.Customers.Instance.RegisterCustomer(UnusedName, s);
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