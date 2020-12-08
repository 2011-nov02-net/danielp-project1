using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace MyStore.WebApp.Models.StoreViewModels
{
    public class StoreViewModel
    {
        private string _name;

        [Display(Name="Location Name")]
        [StringLength(50, ErrorMessage = "The Last Name cannot exceed 50 characters.")]
        [Required]
        public string Name {
            get
            {
                return _name;
            }
            set
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    _name = value;
                }
                else
                {
                    throw new ArgumentException("Store must have valid name");
                }
            }
        }


        [Display(Name = "Number of Orders")]
        public int NumOrders {get; set;}

        [Display(Name = "Items in Stock")]
        public int NumItemsInStock { get; set; }


        [Display(Name = "Orders")]
        public List<OrderViewModel> Orders { get; set; }

    }
}
