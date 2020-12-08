using System;
using System.Collections.Generic;
using System.Linq;
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
                //ensure it's been loaded
                if(c.CustomerOrderHistory.Count() == 0)
                {
                    repo.GetOrderHistory(c);
                }
                
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
        public ActionResult Create([FromServices] IDbRepository repo, CustomerViewModel customer = null)
        {       
            if(customer == null)
            {
                customer = new CustomerViewModel();
            }

            ViewData["Stores"] = GetStoreNames(repo);
            return View(customer);
        }

        // GET: Customer/Details/5
        // view order history and other details
        public ActionResult Details([FromServices] IDbRepository repo, string customerName)
        {
            if (string.IsNullOrWhiteSpace(customerName))
            {
                //todo: set error thing to display on the view, could not find customer
                ModelState.TryAddModelError("customerName", "Invalid name given, it's null, or just space");
                return RedirectToAction(nameof(Choose));
            } else
            {               
                Store.Customer c = repo.GetCustomerByName(new Name(customerName));

                /* get the order history from the repo into the model
                 * incase a new one has been placed since the model was loaded.
                 * Utilized in the mapper to fill in the object.
                 */
                repo.GetOrderHistory(c);

                CustomerViewModel customer = StoreToViewMapper.MapCustomerToView(c);             

                return View(customer);
            }
        }



        // POST: Customer/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([FromServices] IDbRepository repo, CustomerViewModel customer, object nothing = null)
        {
            if (!ModelState.IsValid)
            {
                ViewData["Stores"] = GetStoreNames(repo);
                return View(nameof(Create), customer);
            }

            Store.Location homestore = null;
            if (customer.HomeStore != "None")
            {
                Console.WriteLine(customer.HomeStore);
                try
                {
                    homestore = repo.GetLocation(customer.HomeStore);
                }
                catch (LocationNotFoundException e)
                {
                    Console.Error.WriteLine(e.Message);
                    ModelState.AddModelError("HomeStore", "Location does not exist.");
                    return View(nameof(Create), customer);
                }

            }

            Name custname = new Name(customer.FirstName, customer.LastName, customer.MiddleInitial);
            Console.WriteLine(custname.ToString());

            if (Customers.Instance.HasCustomer(custname))
            {
                // Invalid name, go back
                ModelState.AddModelError("FirstName", "Name Already Exists.");
                ModelState.AddModelError("LastName", "Name Already Exists.");
                ModelState.AddModelError("MiddleInitial", "Name Already Exists.");

                ViewData["Stores"] = GetStoreNames(repo);

                return View(nameof(Create), customer);

            }
            else
            {
                try
                {
                    Store.Customer newcustomer = Customers.Instance.RegisterCustomer(custname, homestore);
                    repo.CreateCustomer(newcustomer);
                } catch(Exception e)
                {
                    Console.Error.WriteLine(e.Message);

                    ModelState.AddModelError("FirstName", "Error creating customer.");

                    ViewData["Stores"] = GetStoreNames(repo);
                    return View(nameof(Create), customer);
                }

                return RedirectToAction("Choose");
            }
        }

        // GET: Customer/Edit/5
        public ActionResult Edit([FromServices] IDbRepository repo, string customerName)
        {
            CustomerViewModel customerViewModel = null;
            if (string.IsNullOrWhiteSpace(customerName))
            {
                Console.Error.WriteLine("Bad Customer Name given.");
                return View(nameof(Choose));
            } else
            {
                customerViewModel = StoreToViewMapper.MapCustomerToView(
                    repo.GetCustomerByName(new Name(customerName))
                    );
            }
            

            ViewData["Stores"] = GetStoreNames(repo);
            return View(customerViewModel ?? new CustomerViewModel());
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
                stores.Add(store.LocationName);
            }

            return stores;
        }
    }
}
