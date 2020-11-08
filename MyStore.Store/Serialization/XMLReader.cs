using System;
using System.Collections.Generic;
using System.Text;

namespace MyStore.Store.Serialization
{
    class XMLReader : IDataReader
    {
        private readonly String DataDir = "../../../Data";


        public void ReadAllData(string dataDir)
        {
            //items -> stores -> customers
            //stores need items for invintory
            //customers need stores for default locations.
            ReadOnlyItems(dataDir);
            ReadOnlyStores(dataDir);
            ReadOnlyCustomers(dataDir);

        }

        public void ReadOnlyItems(string dataDir)
        {
            //open item file

            //register each item one by one
            throw new NotImplementedException();
        }

        public void ReadOnlyStores(string dataDir)
        {
            throw new NotImplementedException();
        }

        public void ReadOnlyCustomers(string dataDir)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dataDir">The directory where serialized data is saved</param>
        public XMLReader(string dataDir)
        {
            this.DataDir = dataDir;
        }
    }
}
