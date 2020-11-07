using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MyStore.Store.Serialization
{
    public class XMLWriter:IDataWriter
    {
        private readonly String DataDir = "../../../Data";

        public void WriteAllData()
        {
            
            Task t1 = WriteItems();
            Task t2 = WriteCustomers();
            Task t3 = WriteLocations();

            t1.Wait();
            t2.Wait();
            t3.Wait();

            throw new NotImplementedException();
        }


        private async Task WriteItems()
        {
            throw new NotImplementedException();
        }

        private async Task WriteCustomers()
        {
            throw new NotImplementedException();
        }

        private async Task WriteLocations()
        {
            throw new NotImplementedException();
    }


    }
}
