using System;
using System.Collections.Generic;

#nullable disable

namespace MyStore.DataModel
{
    public partial class Order
    {
        public Order()
        {
            OrderItems = new HashSet<OrderItem>();
        }

        public int Id { get; set; }
        public int CustomerId { get; set; }
        public string StoreLocation { get; set; }
        public decimal OrderTotal { get; set; }
        public DateTime OrderTime { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual Location StoreLocationNavigation { get; set; }
        public virtual ICollection<OrderItem> OrderItems { get; set; }
    }
}
