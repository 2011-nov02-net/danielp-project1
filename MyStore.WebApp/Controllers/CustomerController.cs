using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyStore.DataModel;
using MyStore.Store;
using MyStore.WebApp.Models.StoreViewModels;

namespace MyStore.WebApp.Controllers
{
    public class CustomerController : Controller
    {
        //GETl  /Customer/Choose
        // log into a customer
        public ActionResult Choose([FromServices] IDbRepository repo, string searchString)
        {
            /* List of customers as CustomerViewModel in an IEnumerable*/
            List<CustomerViewModel> allCustomers = new List<CustomerViewModel>();

            foreach(var c in repo.GetCustomers().ToList())
            {
                allCustomers.Add(StoreToViewMapper.MapCustomerToView(c));
            }

            //filtered by search string
            if( !string.IsNullOrEmpty(searchString))
            {
                allCustomers = allCustomers.Where(
                    cust => cust.Name.Contains(searchString)).ToList();
            }

            return View(allCustomers);
        }

        // GET: Customer/Create
        // create a customer
        public ActionResult Create([FromServices] IDbRepository repo)
        {
            List<String> stores = new List<string>();

            stores.Add("None");

            foreach ( var store in repo.GetLocations().ToList())
            {
                stores.Add(store.Where);
            }

           

            ViewData["Stores"] = stores;
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
        public ActionResult Create([FromServices] IDbRepository repo, IFormCollection collection)
        {
            try
            {
                Console.WriteLine(collection["HomeStore"]);
                Console.WriteLine(collection["Name"]);


                Store.Location homestore = null;
                if (collection["HomeStore"] != "None")
                {
                    homestore = repo.GetLocation(collection["HomeStore"]);
                }

                String middlinit = collection["MiddleInitial"];
                char? middle = null;
                if(middlinit != null && middlinit.Length > 0)
                {
                    middle = middlinit[0];
                }

                Name custname = new Name(collection["FirstName"], collection["LastName"], middle);

                if (Customers.Instance.HasCustomer(custname))
                {
                    // Invalid name, go back

                    return View();

                } else
                {
                    Store.Customer newcustomer = Customers.Instance.RegisterCustomer(custname, homestore);
                    repo.CreateCustomer(newcustomer);


                    return RedirectToAction("Choose");
                }                
            }
            catch
            {
                return View();
            }
        }


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
        

        /* probably wont let deletion happen because it will mess with orders in the DB
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
