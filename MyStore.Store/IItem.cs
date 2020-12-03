using System;
using System.Collections.Generic;
using System.Text;

namespace MyStore.Store
{
    public interface IItem 
    {
        public decimal cost { get; }
        public string name { get;  }

        //private string UPCCode
    }
}
