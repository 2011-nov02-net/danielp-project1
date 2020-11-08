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
            IItem newitem = StoreCatalogue.Instance.RegisterItem(itemname, itemcost);
            //act 
            IItem testitem = StoreCatalogue.Instance.GetItem(itemname);

            //assert
            Assert.NotNull(testitem);
            Assert.Equal(itemname, testitem.name);
            Assert.Equal(itemcost, testitem.cost);
            Assert.Equal(newitem, testitem);

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


        [Fact]
        public void GetItemTest()
        {
            //arrange
            string itemname = "TestItem3";
            IItem newitem = StoreCatalogue.Instance.RegisterItem(itemname, 10);

            //act
            IItem myItem = StoreCatalogue.Instance.GetItem(itemname);

            //assert
            Assert.NotNull(myItem);
            Assert.Equal(itemname, myItem.name);
            Assert.Equal(10, myItem.cost);
            Assert.Equal(newitem, myItem);
        }

        [Fact]
        public void GetNonExistantItemTest()
        {
            //arrange
            //assumes this item hasn't been created yet or ever while running the program so far.
            string itemname = "THIS_ITEM_DOESN'T_EXIST_OR_ELSE_TESTING_WILL_NOT_WORK_SO_DON'T_NAME_ANYTHING_THIS_WEIRD_NAME_PLEASE";


            //act
            //assert
            Assert.Throws<ItemNotFoundException>(() => StoreCatalogue.Instance.GetItem(itemname));
        }
    }
}
