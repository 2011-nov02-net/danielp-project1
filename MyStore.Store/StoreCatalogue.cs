﻿using System;
using System.Collections.Generic;
using System.Text;

namespace MyStore.Store
{
    /// <summary>
    /// A catalogue of all items in the store.
    /// </summary>
    /// <remarks> 
    /// This is a singleton class, intended to be the only one able to instantiate item.
    /// </remarks>
    public class StoreCatalogue
    {
        private Dictionary<String, Item> AllItems;

        private static StoreCatalogue _instance;
        public static StoreCatalogue Instance
        {
            get
            {
                if(_instance is null)
                {
                    _instance = new StoreCatalogue();
                }

                return _instance;
            }
        }

        private StoreCatalogue()
        {
            AllItems = new Dictionary<string, Item>();
        }


        /// <summary>
        /// Gets a reference to an existing item in the catalogue.
        /// </summary>
        /// <param name="itemname">The name of the item</param>
        /// <returns>The item with that name.</returns>
        public Item GetItem(string itemname)
        {
            //check if it's in dictionary

            //if so, return 

            //if not, create, add and return

            throw new NotImplementedException();

            throw new ItemNotFoundException();
        }

        /// <summary>
        /// Addes an item to the store's catalohue of items.
        /// </summary>
        /// <param name="itemName">The human readable name of an item</param>
        /// <param name="itemPrice">The price of the item</param>
        public void RegisterItem(string itemName, float itemPrice)
        {
            //TODO: prevent multiple items with the same name from being added. 
            //possibly create a exception for that.
            throw new NotImplementedException();

        }



        // - - - Item Class - - - //
        /// <summary>
        /// An item with a name and cost
        /// </summary>
        /// <remarks>
        /// This is a strange class, that's kinda a psudo singleton. 
        /// </remarks>
        public class Item
        {
            private static int _NextId = 0;
            /// <summary>
            /// Used as a hashcode and unique identifier.
            /// </summary>
            private int ItemId;
            //TODO: disallow negative costs
            public float cost { get; private set; }
            public string name { get; private set; }


            //COULD ALSO NOT NEED THIS IF WE HAVE ONE INSTANCE OF EACH ITEM 
            //doing these overrides means we can have multiple objects in memory w/ same name
            //not doing this means we would probably 
            //https://ericlippert.com/2011/02/28/guidelines-and-rules-for-gethashcode/

            //should not be different if the object is the same
            // -> should be based on immuteable fields of the object
            public override int GetHashCode()
            {
                return ItemId;
            }

            public override bool Equals(object obj)
            {
                if (obj is Item)
                {
                    Item other = (Item)obj;
                    //check if names are equal
                    this.ItemId = other.ItemId;
                }
                return base.Equals(obj);
            }


            protected Item(string itemname, float itemprice)
            {
                this.name = itemname;
                this.cost = itemprice;

                this.ItemId = _NextId;
                _NextId++;
            }
        }
        // - - - End Item Class - - - //
    }
}
