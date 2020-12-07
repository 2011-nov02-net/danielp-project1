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
        public int amount { get; set; }


        //cost for num items
        [Display(Name = "Cost")]
        [DataType(DataType.Currency)]
        public override decimal cost { get; set; }

    }
}
