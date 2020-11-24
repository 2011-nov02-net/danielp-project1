using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyStore.WebApp.Models.StoreViewModels;

namespace MyStore.WebApp.Controllers
{
    public class CustomerController : Controller
    {
        //GETl  /Customer/Choose
        // log into a customer
        public ActionResult Choose(string searchString)
        {
            /* List of customers as CustomerViewModel in an IEnumerable*/
            IEnumerable<CustomerViewModel> allCustomers = new List<CustomerViewModel>();

            //filtered by search string
            if( !string.IsNullOrEmpty(searchString))
            {
                allCustomers = allCustomers.Where(
                    cust => cust.Name.Contains(searchString));
            }

            return View(allCustomers);
        }

        // GET: Customer/Create
        // create a customer
        public ActionResult Create()
        {
            return View();
        }

        // GET: Customer/Details/5
        // view order history and other details
        public ActionResult Details(int id)
        {
            return View();
        }



        // POST: Customer/Create
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


        /*
        //TODO: decide if we want customers to be editable based on info we wind up displayed
        // could be to change name, but that is a big change in the DB, and not a req
        // GET: Customer/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Customer/Edit/5
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
        // GET: Customer/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Customer/Delete/5
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
