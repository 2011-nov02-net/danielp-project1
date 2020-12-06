using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace MyStore.WebApp.Models.StoreViewModels
{
    public class CustomerViewModel
    {
        //could have a name and these values are taken from it?
        [Display(Name = "Customer Name")]
        [Required]
        public string Name { get; set; }



        [Display(Name="First Name")]
        [Required]
        public string FirstName { get; set; }

        [Display(Name = "Middile Initial")]
        public char? MiddleInitial { get; set; }


        [Display(Name = "Last Name")]
        [Required]
        public string LastName { get; set; }

        //num orders? with fancy get?
        [Display(Name = "Number of Orders")]
        public int NumOrders { get; set; }



        [Display(Name = "Home Store")]
        [Required]
        public string HomeStore { get; set; }

        [Display(Name = "Orders")]
        public List<OrderViewModel> orders { get; set; }
    }
}
