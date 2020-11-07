using System;
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

        public Item GetItem(string itemname)
        {
            //check if it's in dictionary

            //if so, return 

            //if not, create, add and return

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
            float cost;
            string name;


            //COULD ALSO NOT NEED THIS IF WE HAVE ONE INSTANCE OF EACH ITEM 
            //doing these overrides means we can have multiple objects in memory w/ same name
            //not doing this means we would probably 
            //https://ericlippert.com/2011/02/28/guidelines-and-rules-for-gethashcode/

            //should not be different if the object is the same
            // -> should be based on immuteable fields of the object
            public override int GetHashCode()
            {
                return base.GetHashCode();
            }

            public override bool Equals(object obj)
            {
                if (obj is Item)
                {
                    //check if names are equal
                }
                return base.Equals(obj);
            }


            protected Item(string itemname, double itemprice)
            {

            }
        }
        // - - - End Item Class - - - //
    }
}
