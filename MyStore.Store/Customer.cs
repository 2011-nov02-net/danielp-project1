namespace MyStore.Store
{
    public class Customer
    {
        //order history
        OrderHistory orderHistory { get; }

        public readonly Name CustomerName;

        //NOTE: optional req
        #nullable enable
        Location? DefaultStore = null;

        internal Customer(Name name, Location? DefaultStore = null)
        {
            CustomerName = name;
            this.DefaultStore = DefaultStore;
        }
    }
}