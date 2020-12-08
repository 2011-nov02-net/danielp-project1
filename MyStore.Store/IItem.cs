using System;
using System.Collections.Generic;
using System.Text;

namespace MyStore.Store
{
    public interface IItem 
    {
        /// <summary>
        /// The cost of the item.
        /// </summary>
        public decimal cost { get; }
        /// <summary>
        /// The name of the item
        /// </summary>
        public string name { get;  }
    }
}
