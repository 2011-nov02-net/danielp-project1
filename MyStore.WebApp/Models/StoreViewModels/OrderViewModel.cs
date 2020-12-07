using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace MyStore.WebApp.Models.StoreViewModels
{
    public class OrderViewModel
    {
        [Display(Name = "Customer")]
        [Required]
        public string Name { get; set; }


        //add a hidden first middle last set by set name maybe?
        [Display(Name = "Store")]
        [Required]
        public string StoreName { get; set; }

        [Display(Name = "Number of Itmes")]
        public int NumItems { get; set; }

        [Display(Name = "Order Total")]
        public decimal OrderTotal { get; set; }

        [Display(Name = "Order Items")]
        public List<ReceiptItemViewModel> orderitems { get; set; }

        [Display(Name = "Order Time")]
        public DateTime OrderTime { get; set; }
    }
}
