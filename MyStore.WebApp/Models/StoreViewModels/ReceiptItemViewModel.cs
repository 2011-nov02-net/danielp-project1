using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace MyStore.WebApp.Models.StoreViewModels
{
    public class ReceiptItemViewModel : AbstractItemViewModel
    {
        

        [Display(Name = "Number of Items")]
        [Range(0.0, int.MaxValue, ErrorMessage = "The Number of items must be greater than zero.")]
        public int amount { get; set; }


        //cost for num items
        [Display(Name = "Cost")]
        [DataType(DataType.Currency)]
        [Range(0.0, 999999999,  ErrorMessage = "Cost can't be negative.")]
        public override decimal cost { get; set; }

    }
}
