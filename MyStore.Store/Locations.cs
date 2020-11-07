using System;
using System.Collections.Generic;
using System.Text;

namespace MyStore.Store
{
    //todo: singleton
    class Locations
    {
        List<Location> stores;

        private Locations _instance;

        public Locations Instance
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
