using System;
using System.Collections.Generic;
using System.Text;

namespace MyStore.Store
{
    //singleton
    class Locations
    {
        List<Location> stores;

        private static Locations _instance;

        public static Locations Instance
        {
            get
            {
                if (_instance is null)
                {
                    return new Locations();
                } else
                {
                    return _instance;
                }
            }
        }


        private Locations()
        {
            stores = new List<Location>();
        }
    }
}
