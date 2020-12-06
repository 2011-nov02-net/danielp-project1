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
        public ActionResult Index([FromServices] IDbRepository repo, string location, string customer)
        {
            IEnumerable<IOrder> repoorders = repo.GetOrderHistory(repo.GetLocation("Elsewhere")).ToList();
            List<OrderViewModel> orders = new List<OrderViewModel>();
            
            if(repoorders.Count() > 0)
            {
                foreach (var order in repoorders)
                {
                    OrderViewModel orderViewModel = StoreToViewMapper.MapOrderToViewModel(order);
                    orders.Add(orderViewModel);
                }
            }
            return View(orders);
        }

        // GET: OrderController/Details/5
        //TODO: Replace defualt store with something taken from link
        public ActionResult Details([FromServices] IDbRepository repo, string store = "Elsewhere", int id = -1)
        {
            //TODO: REPLACE THIS WITH SOMETHING that just takes one query
            IEnumerable<IOrder> orders = repo.GetOrderHistory( repo.GetLocation(store) );

            /*
            IOrder Order = orders.Where( order => order.Id == id).FirstOrDefault();
            */

            IOrder Order = orders.FirstOrDefault();

            if(Order is null || Order.Items is null || Order.Items.Count < 1)
            {
                Console.Error.WriteLine("Order not found");
                return View(nameof(Index));
            } else
            {

                //TODO: get order items
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
        public ActionResult Create()
        {
            return View();
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
        public ActionResult Edit(int id)
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
    }
}
