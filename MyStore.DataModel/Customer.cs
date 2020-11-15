using System;
using System.Collections.Generic;

#nullable disable

namespace MyStore.DataModel
{
    public partial class Customer
    {
        public Customer()
        {
            Orders = new HashSet<Order>();
        }

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleInitial { get; set; }
        public string StoreLocation { get; set; }

        public virtual Location StoreLocationNavigation { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}
