using MyStore.Store;

namespace MyStore.ConsoleView
{
    internal class SelectStoreMenu : IMenu
    {
        private Customer CurrentCustomer;
        private DataModel.IDbRepository Repo;
        private Customer loggedInCustomer;


        public SelectStoreMenu(DataModel.IDbRepository repo, Customer loggedInCustomer)
        {
            Repo = repo;
            this.loggedInCustomer = loggedInCustomer;
        }


        //TODO: implement the process for selecting a store (see find customer)
        public IMenu DisplayMenu()
        {
            Location selectedStore;

            throw new System.NotImplementedException();

            return new ViewStoreMenu(Repo, CurrentCustomer, selectedStore);
        }
    }
}