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


        [Display(Name = "Number of Orders (NYI)")]
        public int NumOrders
        {
            get
            {
                return 0;

                //if(Locoations.HasLocation(this.Name)) -> 
                //return Store.Orders.Instance.GetOrdersByLocation().Count;
                //possibly from the DB context????
            }

        }


        [Display(Name = "Items in Stock (NYI)")]
        public int NumItemsInStock
        {
            get
            {
                return 0;

                //if(Locations.hasLocation(this.Name)) -> 
                //return Store.Locations.Instance.GetLocation().Stocks.Count;
                //possibly from the DB context????
            }

        }

    }
}
