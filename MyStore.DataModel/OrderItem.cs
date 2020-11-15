using System;
using System.Collections.Generic;

#nullable disable

namespace MyStore.DataModel
{
    public partial class OrderItem
    {
        public int OrderId { get; set; }
        public string ItemId { get; set; }
        public int Quantity { get; set; }

        public virtual Item Item { get; set; }
        public virtual Order Order { get; set; }
    }
}
