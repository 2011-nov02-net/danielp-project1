using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyStore.WebApp.Models.StoreViewModels
{
    public class StockItemViewModel : AbstractItemViewModel
    {

        public override decimal cost { get => base.cost; set => base.cost = value; }
    }
}
