using MyStore.Store;

namespace MyStore.ConsoleView
{
    internal class LoggedInMenu : IMenu
    {
        private Customer LoggedInCustomer;

        public LoggedInMenu(Customer current)
        {
            this.LoggedInCustomer = current;
        }

        public IMenu DisplayMenu()
        {
            throw new System.NotImplementedException();
        }
    }
}