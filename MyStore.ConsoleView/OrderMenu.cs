using MyStore.Store;

namespace MyStore.ConsoleView
{
    internal class OrderMenu : IMenu
    {
        private Customer currentCustomer;
        private Location selectedStore;
        private Order currentOrder;
        private DataModel.IDbRepository Repo;

        public OrderMenu(DataModel.IDbRepository repo, Customer currentCustomer, Location selectedStore)
        {
            Repo = repo;
            this.currentCustomer = currentCustomer;
            this.selectedStore = selectedStore;
        }

        public IMenu DisplayMenu()
        {
            throw new System.NotImplementedException();

            //cancel order
            //TODO: undo changes from the order in the model
            return new ViewStoreMenu(Repo, currentCustomer, selectedStore);
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