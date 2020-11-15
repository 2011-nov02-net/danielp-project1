using MyStore.Store;

namespace MyStore.ConsoleView
{
    internal class SelectStoreMenu : IMenu
    {
        private Customer CurrentCustomer;

        public SelectStoreMenu(Customer customer)
        {
            this.CurrentCustomer = customer;
        }


        //TODO: implement the process for selecting a store (see find customer)
        public IMenu DisplayMenu()
        {
            Location selectedStore;

            throw new System.NotImplementedException();

            return new ViewStoreMenu(CurrentCustomer, selectedStore);
        }
    }
}