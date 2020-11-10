using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace MyStore.Store
{
    /// <summary>
    /// A catalogue of all items in the store.
    /// </summary>
    /// <remarks> 
    /// This is a singleton class, intended to be the only one able to instantiate item.
    /// </remarks>
    public class StoreCatalogue : ISerializable
    {
        #region Store Catalogue
        private Dictionary<String, Item> AllItems;

        #region Singleton code and Constructor
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
        #endregion


        /// <summary>
        /// Gets a reference to an existing item in the catalogue.
        /// </summary>
        /// <param name="itemname">The name of the item</param>
        /// <returns>The item with that name.</returns>
        public IItem GetItem(string itemname)
        {
            Item desiredItem;

            //check if it's in dictionary
            if (AllItems.TryGetValue(itemname, out desiredItem))
            {
                //if so, return
                return desiredItem;
            } else
            {
                //else throw exception 
                throw new ItemNotFoundException();
            }       
        }

        /// <summary>
        /// Test if an item exists.
        /// </summary>
        /// <param name="itemname">The item's name</param>
        /// <returns>True if the item has been registered with the item catalogue.</returns>
        public bool ItemExists(string itemname)
        {
            return AllItems.ContainsKey(itemname); 
        }

        /// <summary>
        /// Addes an item to the store's catalohue of items.
        /// </summary>
        /// <param name="itemName">The human readable name of an item</param>
        /// <param name="itemPrice">The price of the item</param>
        /// <returns>The newly created item.</returns>
        public IItem RegisterItem(string itemName, float itemPrice)
        {
            // prevents multiple items with the same name from being added.
            if (!AllItems.ContainsKey(itemName))
            {
                Item newitem = new Item(itemName, itemPrice);
                AllItems.Add(itemName, newitem);
                return newitem;
            } else
            {
                //possibly create a exception for that.
                throw new ArgumentException();
            }
        }

        /// <summary>
        /// Get a read only list of every item.
        /// </summary>
        /// <returns>A read only list of all items.</returns>
        public IReadOnlyCollection<IItem> GetAllItems()
        {
            return AllItems.Values;
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
                throw new System.ArgumentNullException("info");
            ((ISerializable)AllItems).GetObjectData(info, context);
        }

        #endregion

        #region Item
        // - - - Item Class - - - //
        /// <summary>
        /// An item with a name and cost >= 0.
        /// </summary>
        /// <remarks>
        /// This Class is only ment to be instantiated by StoreCatalogue. 
        /// </remarks>
        private class Item : IItem
        {
            private static int _NextId = 0;
            /// <summary>
            /// Used as a hashcode and unique identifier.
            /// </summary>
            private int ItemId;
            // disallow negative costs

            private float _cost;
            public float cost 
            {
                get
                {
                    return _cost;
                } 
                private set
                {
                    if(cost >= 0)
                    {
                        this._cost = value;
                    }
                    else
                    {
                        throw new ArgumentOutOfRangeException("Error: Cannot have negative item prices.");
                    }
                }                   
            }
            public string name { get; }

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

            public void GetObjectData(SerializationInfo info, StreamingContext context)
            {
                if (info == null)
                    throw new System.ArgumentNullException("info");

                info.AddValue("Name", name);
                info.AddValue("Cost", cost);
            }

            protected internal Item(string itemname, float itemprice)
            {
                this.name = itemname;
                this.cost = itemprice;

                this.ItemId = _NextId;
                _NextId++;
            }
        }
        // - - - End Item Class - - - //
        #endregion
    }
}
