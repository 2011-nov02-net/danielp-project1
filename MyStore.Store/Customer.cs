using System.Runtime.Serialization;

namespace MyStore.Store
{
    public class Customer : ISerializable
    {
        //order history
        public OrderHistory OrderHistory { get; }

        public readonly Name CustomerName;

        //NOTE: optional req
        #nullable enable
        Location? DefaultStore = null;

        internal Customer(Name name, Location? DefaultStore = null)
        {
            CustomerName = name;
            this.DefaultStore = DefaultStore;
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Name", CustomerName);

            if(DefaultStore != null)
            {
                info.AddValue("DefaultStore", DefaultStore);
            }
        }
    }
}