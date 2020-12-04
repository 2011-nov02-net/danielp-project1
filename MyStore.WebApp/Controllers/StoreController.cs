using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyStore.DataModel;
using MyStore.WebApp.Models.StoreViewModels;

namespace MyStore.WebApp.Controllers
{
    public class StoreController : Controller
    {
        // GET: Store/Stores
        //view list of stores
        public ActionResult Stores([FromServices] IDbRepository repo )
        {
            List<StoreViewModel> stores = new List<StoreViewModel>();

            foreach (var x in repo.GetLocations().ToList())
            {
                stores.Add(StoreToViewMapper.MapLocationToStore(x));
            }
            
            return View(stores);
        }

        // GET: StoreController/Details/5
        //view a particular store's orders
        public ActionResult Orders(int id)
        {
            //TODO: redirect to orders filtered to store
            return RedirectToAction("Index", "Orders", id);
        }

        public ActionResult Stock(int id)
        {
            List<StockItemViewModel> stocks = new List<StockItemViewModel>();

            //TODO: get stocks

            return View(stocks);
        }    
    }
}
