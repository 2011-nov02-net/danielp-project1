using System;
using Xunit;
using MyStore.Store;
using static MyStore.Store.StoreCatalogue;
using System.Diagnostics;
using Xunit.Sdk;

namespace MyStore.Store.Tests
{
    public class CatalogueTests
    {
        [Fact]
        public void RegisterValidItem()
        {
            //arrange
            string itemname = "TestItem";
            float itemcost = 10;
            StoreCatalogue.Instance.RegisterItem(itemname, itemcost);
            //act 
            Item testitem = StoreCatalogue.Instance.GetItem(itemname);

            //assert
            Assert.NotNull(testitem);
            Assert.Equal(itemname, testitem.name);
            Assert.Equal(itemcost, testitem.cost);

        }

        [Fact]
        public void RegisterInvalidItem()
        {
            //arrange
            string itemname = "TestItem2";
            StoreCatalogue.Instance.RegisterItem(itemname, 0);

            //act
            //assert

            //todo make this more specific
            Assert.ThrowsAny<Exception>(()=>StoreCatalogue.Instance.RegisterItem(itemname, 0));
        }
    }
}
