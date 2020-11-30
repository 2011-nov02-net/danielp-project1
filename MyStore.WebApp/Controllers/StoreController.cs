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
            return View();
        }

        public ActionResult Stock(int id)
        {
            return View();
        }

        /*
        // GET: StoreController/Create
        public ActionResult Create()
        {
            return View();
        }
        

        // POST: StoreController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
        */


        /*
        // GET: StoreController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: StoreController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
        */


        /*
        // GET: StoreController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: StoreController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
        */
    }
}
