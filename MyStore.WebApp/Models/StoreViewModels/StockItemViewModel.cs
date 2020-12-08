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
        [DataType(DataType.Currency)]
        [Range(0.0, 999999999, ErrorMessage = "Cost can't be negative.")]
        public override decimal cost { get => base.cost; set => base.cost = value; }

        [Display(Name = "Amount In Stock")]
        [Range(0.0, int.MaxValue, ErrorMessage = "The Number of items must be greater than zero.")]
        public int NumInStock { get; set; }
    }
}
