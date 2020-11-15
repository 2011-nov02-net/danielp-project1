using System;
using System.Collections.Generic;

#nullable disable

namespace MyStore.DataModel
{
    public partial class Invintory
    {
        public string StoreLocation { get; set; }
        public string ItemName { get; set; }
        public int Quantity { get; set; }

        public virtual Item ItemNameNavigation { get; set; }
        public virtual Location StoreLocationNavigation { get; set; }
    }
}
