using MyStore.Store;

namespace MyStore.ConsoleView
{
    internal class ViewStoreMenu : IMenu
    {
        private Customer currentCustomer;
        private Location selectedStore;
        private DataModel.IDbRepository Repo;


        public ViewStoreMenu(DataModel.IDbRepository repo, Customer currentCustomer, Location selectedStore)
        {
            Repo = repo;
            this.currentCustomer = currentCustomer;
            this.selectedStore = selectedStore;
        }

        public IMenu DisplayMenu()
        {
            throw new System.NotImplementedException();
            return new OrderMenu(Repo, currentCustomer, selectedStore);
        }

        //todo: Implement view stock
        private void ViewStock()
        {
            throw new System.NotImplementedException();
        }

        //TODO: implement view store order history
        private void ViewStoreOrderHistory()
        {
            throw new System.NotImplementedException();
        }
    }
}