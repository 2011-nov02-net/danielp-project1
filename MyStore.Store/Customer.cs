namespace MyStore.Store
{
    public class Customer
    {
        // todo TODO: TODO stuff
        //order history

        public struct Name
        {
            string First;
            char? MiddleInitial;
            string Last;
        };

        readonly Name CustomerName;

        #nullable enable
        Location? DefaultStore;
    }
}