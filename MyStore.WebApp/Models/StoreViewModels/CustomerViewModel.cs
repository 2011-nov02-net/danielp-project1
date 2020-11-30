using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace MyStore.WebApp.Models.StoreViewModels
{
    public class CustomerViewModel
    {
        [Display(Name="Customer Name")]
        public string Name { get; set; }

        //num orders? with fancy get?
        [Display(Name = "Number of Orders (NYI)")]
        public int NumOrders { get; set; }
    }
}
