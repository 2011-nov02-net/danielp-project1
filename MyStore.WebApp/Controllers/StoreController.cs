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
            List<StoreViewModel> stores = LoadandRetrieveStoreData(repo);
            return View(stores);
        }

       

        // GET: StoreController/Details/5
        //view a particular store's orders
        public ActionResult Orders(string store)
        {
            //redirect to orders filtered to store
            return RedirectToAction("Index", "Order", new { store = store});
        }

        public ActionResult Stock([FromServices] IDbRepository repo, string store)
        {
            if (!string.IsNullOrWhiteSpace(store))
            {
                Store.Location modelLocation = repo.GetLocation(store);
                List<StockItemViewModel> stocks = new List<StockItemViewModel>();

                foreach(var item in modelLocation.GetAllStock())
                {
                    stocks.Add(StoreToViewMapper.MapStockToStockItem(item));
                }

                return View(stocks);
            } else
            {
                ModelState.TryAddModelError("BadStore", "Error: bad store name given.");
                return View(nameof(Stores), LoadandRetrieveStoreData(repo));
            }
            
        }

        //GET: Store/Details?&store=store
        public ActionResult Details([FromServices] IDbRepository repo, string store)
        {
            if (!string.IsNullOrWhiteSpace(store))
            {
                var s = repo.GetLocation(store);

                if(s.LocationOrderHistory.Count() == 0)
                {
                    repo.GetOrderHistory(s);
                }

                var viewModel = StoreToViewMapper.MapLocationToStore(s);

                return View(viewModel);
            } else
            {
                ModelState.AddModelError("BadName", "Bad store name given");
                return RedirectToAction("Stores");
            }
        }

        /// <summary>
        /// Get's the data needed by the Stores action.
        /// </summary>
        /// <param name="repo"></param>
        /// <returns>List of all stores</returns>
        private static List<StoreViewModel> LoadandRetrieveStoreData(IDbRepository repo)
        {
            List<StoreViewModel> stores = new List<StoreViewModel>();

            foreach (var x in repo.GetLocations().ToList())
            {
                repo.GetOrderHistory(x);

                stores.Add(StoreToViewMapper.MapLocationToStore(x));
            }

            return stores;
        }
    }
}
