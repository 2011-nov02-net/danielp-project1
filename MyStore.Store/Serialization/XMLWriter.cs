using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace MyStore.Store.Serialization
{
    public class XMLWriter:IDataWriter
    {
        private const String DataDir = "../../../Data/";
        private const String ItemsFile = "CatalogueItems.xml";

        public void WriteAllData()
        {            
            WriteItems();
            WriteCustomers();
            WriteLocations();
        }


        private void WriteItems()
        {
            Type itemtype = typeof(StoreCatalogue);

            //https://docs.microsoft.com/en-us/dotnet/api/system.xml.xmlwritersettings?view=netcore-3.1 
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.NewLineOnAttributes = true;


            using (XmlWriter writer = XmlWriter.Create(DataDir + ItemsFile, settings))
            {
                XmlSerializer serializer = new XmlSerializer(itemtype);
                serializer.Serialize(writer, StoreCatalogue.Instance);
            }
            
        }

        private void WriteCustomers()
        {
            throw new NotImplementedException();
        }

        private void WriteLocations()
        {
            throw new NotImplementedException();
        }


    }
}
