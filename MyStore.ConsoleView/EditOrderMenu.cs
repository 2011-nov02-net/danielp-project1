using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyStore.DataModel;
using MyStore.Store;

namespace MyStore.ConsoleView
{
    class EditOrderMenu : IMenu
    {
        public EditOrderMenu(IDbRepository repo, Store.Customer currentCustomer, Store.Location selectedStore, Store.Order currentOrder)
        {
            Repo = repo;
            this.currentCustomer = currentCustomer;
            this.selectedStore = selectedStore;
            this.currentOrder = currentOrder;
        }

        public IDbRepository Repo { get; }
        public Store.Customer currentCustomer { get; }
        public Store.Location selectedStore { get; }
        public Store.Order currentOrder { get; }

        public IMenu DisplayMenu()
        {

            int userchoice;
            do
            {
                Console.WriteLine("Please choose from the following options:");
                Console.WriteLine("{C}ancel - Cancel the current order.");
                Console.WriteLine("{A}dd - add a new item to the order.");             
                Console.WriteLine("{R}emove item - remove an item from the order.");
                Console.WriteLine("{D}one - Place the order..");
            }
            while (!Program.ValidOption(Console.ReadLine(),
                                        new List<string> { "c", "cancel", "a", "add", "r", "remove", "d", "done" },
                                        out userchoice));


            switch (userchoice)
            {
                //cancnel
                case 0:
                case 1:
                    //NOTE: ORDERS don't modify store amounts, so fine to just like ignore hthe order

                    return new ViewStoreMenu(Repo, currentCustomer, selectedStore);
                    break;
                //add item
                case 2:
                case 3:
                    this.AddItem();
                    return this;
                    break;
                //remove item
                case 4:
                case 5:
                    this.RemoveItem();
                    return this;
                    break;
                //done
                case 6:
                case 7:
                    //send to database
                    try
                    {
                        Repo.PlaceOrder(currentOrder);
                        currentOrder.FinallizeOrder();
                    } catch(Exception e)
                    {
                        Console.Out.WriteLine("Error: " + e?.Message);
                    }
                    return new ViewStoreMenu(Repo, currentCustomer, selectedStore);
                    break;
                //Some error has happened, just go back and do this menu over again.
                default:
                    return this;
                    break;
            }
        }



        private void AddItem()
        {
            List<ItemCount> stocks = selectedStore.GetLocationStock()
                .Where(curritem => curritem.Count > 0)
                .ToList();

            List<ItemCount> toremove = new List<ItemCount>();
            //for every item in store's stocks
            foreach(ItemCount ic in stocks)
            {
                foreach(ItemCount oic in currentOrder.Items)
                {
                    //if that item is also in the order
                    if(oic.ThisItem == ic.ThisItem)
                    {
                        //plan on removing the listing from stocks
                        toremove.Add(ic);
                    }
                }
            }

            //remove them
            foreach(ItemCount ic in toremove)
            {
                stocks.Remove(ic);
            }
           

            //would you like to see what's in stock? y/n
            Console.WriteLine($"Would you like to see a list of all items in stock at {selectedStore.Where}? y/n");
            int choice = Program.ValidYesNoOption();

            if (choice == 0) //yes
            {
                this.ViewSomeStock(stocks);
            }


            List<string> items = new List<string>();
            foreach (ItemCount ic in stocks)
            {
                //add each item to the choices
                items.Add(ic.ThisItem.name.ToLower().Trim());
            }

            int itemindex;

            do
            {
                Console.Out.WriteLine("\nType the name of the item you wish to buy");
            } while (!Program.ValidOption(Console.ReadLine(), items, out itemindex));

            /*
             * At this point, stocks should be a list of all items with at least one in stock, items should be an
             * index aligned list of their names
             * and itemindex should be the index of the item name that the customer types.
             */

            Console.WriteLine($"How many of {items[itemindex]}s would you like to buy?");
            int amount = Program.GetIntegerAmount(max: stocks[itemindex].Count, min: 0);

            //Order currentOrder = selectedStore.CreateNewOrder(stocks[itemindex].ThisItem, amountofitem, currentCustomer);
            currentOrder.AddItem(stocks[itemindex].ThisItem, amount);
        }



        private void RemoveItem()
        {
            Console.Out.WriteLine("Which item would you like to remove?");

            List<ItemCount> orderitems = currentOrder.Items.ToList();
            List<string> itemNames = new List<string>();
            foreach(ItemCount ic in orderitems)
            {
                itemNames.Add(ic.ThisItem.name);

                //print items/amounts
                Console.WriteLine($"\t{ic.ThisItem.name,20}\tx{ic.Count}");
            }


            int choice;
            do
            {

            }
            while (!Program.ValidOption(Console.ReadLine(), itemNames, out choice));


            currentOrder.RemoveItem(itemNames[choice]);
            return;
        }

        public void ViewSomeStock(List<ItemCount> stockItmes)
        {

            Console.WriteLine($"{selectedStore.Where} has the following currently in stock and not already in the order:");
            foreach (ItemCount ic in stockItmes)
            {
                Console.WriteLine($"{ic.ThisItem.name,20}\t{ic.ThisItem.cost:C}\tWith {ic.Count} in stock.");
            }
            Console.WriteLine("\n");
        }
    }
}
