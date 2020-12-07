using System;
using System.Runtime.Serialization;

namespace MyStore.Store
{
    [Serializable]
    public class LocationNotFoundException : ArgumentException
    {
        public LocationNotFoundException()
        {
        }

        public LocationNotFoundException(string message) : base(message)
        {
        }

        public LocationNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected LocationNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}