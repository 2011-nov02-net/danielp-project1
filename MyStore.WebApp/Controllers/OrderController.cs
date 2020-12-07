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
    public class OrderController : Controller
    {
        // GET: OrderController
        public ActionResult Index([FromServices] IDbRepository repo, string store, string customer)
        {
            IEnumerable<IOrder> repoorders = repo.GetAllOrders().ToList();
            List<OrderViewModel> orders = new List<OrderViewModel>();
            
            //convert to view model
            if(repoorders.Count() > 0)
            {
                foreach (var order in repoorders)
                {
                    OrderViewModel orderViewModel = StoreToViewMapper.MapOrderToViewModel(order);
                    orders.Add(orderViewModel);
                }
            }

            //filter by customer and/or store
            if (!string.IsNullOrWhiteSpace(store))
            {
                orders = orders.Where(order => order.StoreName.Contains(store)).ToList();
            }

            if (!string.IsNullOrWhiteSpace(customer))
            {
                orders = orders.Where(order => order.Name.Contains(customer)).ToList();
            }

            return View(orders);
        }

        // GET: OrderController/Details/5
        //TODO: Replace defualt store with something taken from link
        public ActionResult Details([FromServices] IDbRepository repo, int ID)
        {
            IOrder Order = Orders.Instance.GetOrderByID(ID);


            if(Order is null || Order.Items is null || Order.Items.Count < 1)
            {
                Console.Error.WriteLine("Order not found");
                //todo: set error text
                return View(nameof(Index));
            } else
            {
                List<ReceiptItemViewModel> OrderItems = new List<ReceiptItemViewModel>();

                foreach(var item in Order.Items)
                {
                    ReceiptItemViewModel receptItemViewModel = StoreToViewMapper.MapOrderEntryToRecieptItem(item);
                    OrderItems.Add(receptItemViewModel);
                }

                ViewData["TotalCost"] = Order.Cost;
                ViewData["Customer"] = Order.Customer.CustomerName.ToString();
                ViewData["Store"] = Order.OrderLoc.Where;

                return View(OrderItems);
            }

        }

        // GET: OrderController/Create
        public ActionResult Create([FromServices] IDbRepository repo)
        {
            ViewData["Customers"] = GetCustomerNames(repo);
            ViewData["Stores"] = GetStoreNames(repo);
            return View(new OrderViewModel());
        }

        // POST: OrderController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Edit));
            }
            catch
            {
                return View();
            }
        }

        //edit the items in the order before placing it.
        // GET: OrderController/Edit/5
        public ActionResult Edit(string StoreName, string Name)
        {
            return View();
        }

        // POST: OrderController/Edit/5
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



        //Coppied from CustomerController
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



        /// <summary>
        /// Get a list of all store names, and convert them to something displayable in a view. 
        /// </summary>
        /// <param name="repo">DB Repository.</param>
        /// <returns>List of strings, representing all existing store names.</returns>
        private List<String> GetCustomerNames(IDbRepository repo)
        {
            List<String> Customers = new List<string>();

            foreach (var cust in repo.GetCustomers().ToList())
            {
                Customers.Add(cust.CustomerName.ToString());
            }

            return Customers;
        }
    }
}
