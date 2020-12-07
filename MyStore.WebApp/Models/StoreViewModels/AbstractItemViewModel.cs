using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace MyStore.WebApp.Models.StoreViewModels
{
    public class AbstractItemViewModel
    {

        [Display(Name = "Item")]
        public string name { get; set; }

        [Display(Name = "Cost")]
        [DataType(DataType.Currency)]
        public virtual decimal cost { get; set; }
    }
}
