using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace MyStore.Store
{
    public interface IItem : ISerializable
    {
        public decimal cost { get; }
        public string name { get;  }

        //private string UPCCode
    }
}
