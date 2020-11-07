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
            throw new NotImplementedException();
        }

        public void ReadOnlyItems(string dataDir)
        {
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
