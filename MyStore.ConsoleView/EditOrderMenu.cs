using System;
using System.Collections.Generic;
using System.Text;
using MyStore.DataModel;

namespace MyStore.ConsoleView
{
    class EditOrderMenu : IMenu
    {
        public EditOrderMenu(IDbRepository repo, Store.Customer currentCustomer, Store.Location selectedStore, Store.Order currentOrder)
        {
            Repo = repo;
            CurrentCustomer = currentCustomer;
            SelectedStore = selectedStore;
            CurrentOrder = currentOrder;
        }

        public IDbRepository Repo { get; }
        public Store.Customer CurrentCustomer { get; }
        public Store.Location SelectedStore { get; }
        public Store.Order CurrentOrder { get; }

        public IMenu DisplayMenu()
        {
            throw new NotImplementedException();
        }


        private void AddItem()
        {
            throw new System.NotImplementedException();
        }

        private void RemoveItem()
        {
            throw new System.NotImplementedException();
        }

        private void EditOrderQuantity()
        {
            throw new System.NotImplementedException();
        }
    }
}
