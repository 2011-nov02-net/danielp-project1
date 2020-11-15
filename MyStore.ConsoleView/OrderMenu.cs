using MyStore.Store;

namespace MyStore.ConsoleView
{
    internal class OrderMenu : IMenu
    {
        private Customer currentCustomer;
        private Location selectedStore;
        private Order currentOrder;

        public OrderMenu(Customer currentCustomer, Location selectedStore)
        {
            this.currentCustomer = currentCustomer;
            this.selectedStore = selectedStore;
        }

        public IMenu DisplayMenu()
        {
            throw new System.NotImplementedException();

            //cancel order
            //TODO: undo changes from the order in the model
            return new ViewStoreMenu(currentCustomer, selectedStore);
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