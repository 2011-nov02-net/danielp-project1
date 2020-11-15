using MyStore.Store;

namespace MyStore.ConsoleView
{
    internal class ViewStoreMenu : IMenu
    {
        private Customer currentCustomer;
        private Location selectedStore;

        public ViewStoreMenu(Customer currentCustomer, Location selectedStore)
        {
            this.currentCustomer = currentCustomer;
            this.selectedStore = selectedStore;
        }

        public IMenu DisplayMenu()
        {
            throw new System.NotImplementedException();
            return new OrderMenu(currentCustomer, selectedStore);
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