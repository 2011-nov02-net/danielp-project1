using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyStore.WebApp.Models.StoreViewModels
{
    public class StockItemViewModel : AbstractItemViewModel
    {
        [Display(Name = "Cost")]
        public override decimal cost { get => base.cost; set => base.cost = value; }

        [Display(Name = "Amount In Stock")]
        public int NumInStock { get; set; }
    }
}
