using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyStore.DataModel;
using MyStore.Store;
using MyStore.WebApp.Models.StoreViewModels;

namespace MyStore.WebApp.Controllers
{
    public class CustomerController : Controller
    {
        //GET  /Customer/Choose
        /// <summary>
        /// Display a list of all customers that you can search by name
        /// </summary>
        /// <param name="repo">The repository of info, pulled from DB</param>
        /// <param name="searchString">Filters list of customers to only the ones with the searchString in their name</param>
        /// <returns></returns>
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
            ViewData["Stores"] = GetStoreNames(repo);
            return View();
        }

        // GET: Customer/Details/5
        // view order history and other details
        public ActionResult Details([FromServices] IDbRepository repo, string customerName)
        {
            if (string.IsNullOrWhiteSpace(customerName))
            {
                //todo: set error thing to display on the view, could not find customer

                return View(nameof(Choose));
            } else
            {
                repo.GetCustomerByName(new Name(customerName));


                return View();
            }
        }



        // POST: Customer/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([FromServices] IDbRepository repo, IFormCollection collection)
        {
            //TODO: check the form status for errors.

            try
            {


                Store.Location homestore = null;
                if (collection["HomeStore"] != "None")
                {
                    Console.WriteLine(collection["HomeStore"]);
                    homestore = repo.GetLocation(collection["HomeStore"]);
                }

                String middlinit = collection["MiddleInitial"];
                char? middle = null;
                if(middlinit != null && middlinit.Length > 0)
                {
                    middle = middlinit[0];
                }

                Name custname = new Name(collection["FirstName"], collection["LastName"], middle);
                Console.WriteLine(custname.ToString());

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

        // GET: Customer/Edit/5
        public ActionResult Edit([FromServices] IDbRepository repo, string customerName)
        {
            if (string.IsNullOrWhiteSpace(customerName))
            {
                Console.Error.WriteLine("Bad Customer Name given.");
                return View(nameof(Choose));
            } else
            {
                //todo: fix so that this will work, either split up first/middle/last or do something else, or just 
                //accept that space will be converted to +
                customerName = HttpUtility.UrlDecode(customerName);
                repo.GetCustomerByName(new Name(customerName));
            }
            

            ViewData["Stores"] = GetStoreNames(repo);
            return View();
        }

        // POST: Customer/Edit/5
        //TODO: store old name so we know who to update.
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


        /// <summary>
        /// Get a list of all store names, and convert them to something displayable in a view. 
        /// </summary>
        /// <param name="repo">DB Repository.</param>
        /// <returns>List of strings, representing all existing store names.</returns>
        private List<String> GetStoreNames(IDbRepository repo)
        {
            List<String> stores = new List<string>();

            foreach (var store in repo.GetLocations().ToList())
            {
                stores.Add(store.Where);
            }

            return stores;
        }
    }
}
