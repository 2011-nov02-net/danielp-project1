using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MyStore.Store.Serialization
{
    /// <summary>
    /// Classes responsible for reading data into objects
    /// </summary>
    interface IDataReader
    {
        public void ReadAllData(String dataDir);

        public void ReadOnlyItems(String dataDir);
        public void ReadOnlyStores(String dataDir);
        public void ReadOnlyCustomers(String dataDir);
    }
}
