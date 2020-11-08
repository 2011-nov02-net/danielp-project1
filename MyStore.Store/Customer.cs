namespace MyStore.Store
{
    public class Customer
    {
        //order history
        OrderHistory orderHistory;

        readonly Name CustomerName;

        //NOTE: optional req
        #nullable enable
        Location? DefaultStore = null;

        public Customer(Name name, Location? DefaultStore = null)
        {
            CustomerName = name;
            this.DefaultStore = DefaultStore;
        }
    }
}