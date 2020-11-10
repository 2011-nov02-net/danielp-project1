using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace MyStore.Store.Serialization
{
    class XMLWriterAsynch
    {
        private const String DataDir = "../../../Data/";
        private const String ItemsFile = "CatalogueItems.xml";

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
            Type itemtype = typeof(StoreCatalogue);

            //https://docs.microsoft.com/en-us/dotnet/api/system.xml.xmlwritersettings?view=netcore-3.1 
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Async = true;
            settings.Indent = true;
            settings.NewLineOnAttributes = true;


            using (XmlWriter writer = XmlWriter.Create(DataDir + ItemsFile, settings))
            {
                //TODO: try catch this
                //todo: make this work.
                Task task = writer.WriteStartDocumentAsync();
                XmlSerializer serializer = new XmlSerializer(itemtype);
                serializer.Serialize(writer, StoreCatalogue.Instance);
            }

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
