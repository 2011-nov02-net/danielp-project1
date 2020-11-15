using System;
using System.Collections.Generic;

#nullable disable

namespace MyStore.DataModel
{
    public partial class Location
    {
        public Location()
        {
            Customers = new HashSet<Customer>();
            Invintories = new HashSet<Invintory>();
            Orders = new HashSet<Order>();
        }

        public string LocationName { get; set; }

        public virtual ICollection<Customer> Customers { get; set; }
        public virtual ICollection<Invintory> Invintories { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}
