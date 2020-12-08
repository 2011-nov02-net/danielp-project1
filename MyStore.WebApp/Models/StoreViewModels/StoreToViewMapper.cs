using System;
using System.Collections.Generic;
using System.Linq;
using MyStore.Store;

namespace MyStore.WebApp.Models.StoreViewModels
{
    public class StoreToViewMapper
    {
        public static CustomerViewModel MapCustomerToView(Store.Customer StoreCust)
        {
            CustomerViewModel customerViewModel = new CustomerViewModel();
            customerViewModel.FirstName = StoreCust.CustomerName.First;
            customerViewModel.MiddleInitial = StoreCust.CustomerName.MiddleInitial;
            customerViewModel.LastName = StoreCust.CustomerName.Last;
            customerViewModel.Name = StoreCust.CustomerName.ToString();
            customerViewModel.NumOrders = StoreCust.CustomerOrderHistory.Count();
            customerViewModel.HomeStore = StoreCust.DefaultStore?.LocationName ?? "None";
            customerViewModel.orders = new List<OrderViewModel>();
            foreach(var order in StoreCust.CustomerOrderHistory)
            {
                customerViewModel.orders.Add(MapOrderToViewModel(order));
            }

            return customerViewModel;
        }


        public static ReceiptItemViewModel MapOrderEntryToRecieptItem(Store.ItemCount orderItem)
        {
            ReceiptItemViewModel receiptItemView = new ReceiptItemViewModel();
            receiptItemView.name = orderItem.ThisItem.name;
            receiptItemView.amount = orderItem.Count;
            receiptItemView.cost = orderItem.ThisItem.cost * receiptItemView.amount;
            return receiptItemView;
        }

        public static StockItemViewModel MapStockToStockItem(Store.ItemCount stockItem)
        {
            StockItemViewModel viewStockItem = new StockItemViewModel();
            viewStockItem.cost = stockItem.ThisItem.cost;
            viewStockItem.name = stockItem.ThisItem.name;
            viewStockItem.NumInStock = stockItem.Count;
            return viewStockItem;
        }

        public static StoreViewModel MapLocationToStore(Store.Location l)
        {
            StoreViewModel storeViewModel = new StoreViewModel();

            storeViewModel.Name = l.LocationName;
            storeViewModel.NumItemsInStock = l.GetAllStock().ToList()
                .Where(item => 
                    item.Count > 0)
                .Count();

            storeViewModel.Orders = new List<OrderViewModel>();
            foreach (var order in l.LocationOrderHistory)
            {
                storeViewModel.Orders.Add(MapOrderToViewModel(order));
            } 

            storeViewModel.NumOrders = storeViewModel.Orders.Count();

            return storeViewModel;
        }


        public static OrderViewModel MapOrderToViewModel(Store.IOrder storeorder)
        {
            OrderViewModel ovm = new OrderViewModel();
            ovm.Name = storeorder.Customer.CustomerName.ToString();
            ovm.NumItems = storeorder.Items.Count();
            ovm.OrderTotal = storeorder.Cost;
            ovm.StoreName = storeorder.OrderLoc.LocationName;
            ovm.OrderTime = storeorder.Time;
            ovm.ID = storeorder.ID;

            return ovm;
        }
    }
}
