using MyStore.Store;

namespace MyStore.ConsoleView
{
    internal class CreateCustomer : IMenu
    {
        public IMenu DisplayMenu()
        {
            Customer Current = null;
            Name customerName;

            //TODO: implement function
            //possibly pull out into it's own function?
            do
            {
                //get first name

                //ask if want to include middle initial
                //get middle initial

                //get last name
            } while (false); //confirm the name is unique (if not handled by class already).

            //TODO: probably move into the above
            Current = Customers.Instance.RegisterCustomer(Current);

            return new LoggedInMenu(Current);
        }
    }
}