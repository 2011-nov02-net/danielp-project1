using System;
using System.Collections.Generic;
using MyStore.Store;

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
            bool FoundCustomer = false;
            Customer Current = null;

            do
            {
                //TODO: get first name

                //TODO: ask if middle inital
                    //if so, get it

                //TODO: get last name

                //TODO: check if customer exists in the model, if so found customer = true
            }
            while (!FoundCustomer);


            
            return new LoggedInMenu(Repo, Current);
        }
    }
}