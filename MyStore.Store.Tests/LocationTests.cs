using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace MyStore.Store.Tests
{
    public class LocationTests
    {
        [Fact]
        public void GetEmptyStockTest()
        {
            //arrange
            Location l = Locations.Instance.RegisterLocation("testLoc1");
            //act
            int amount = l.CheckStock("Nothing");
            //assert
            Assert.Equal(0, amount);
        }

        [Fact]
        public void AddStockTest()
        {
            //arrange
            Location l = Locations.Instance.RegisterLocation("testLoc2");
            string testitemName = "TestSomething1";
            int InitialAamount = l.CheckStock(testitemName);
            Assert.Equal(0, InitialAamount); //assume there is zero of nothing initially
            StoreCatalogue.Instance.RegisterItem(testitemName, 50);

            int amountToAdd = 3;

            //act
            int finalamount = l.AddInventory(testitemName, amountToAdd);

            //assert
            Assert.Equal(amountToAdd, finalamount);
        }

        [Fact]
        public void GetStockTest()
        {
            //arrange
            Location l = Locations.Instance.RegisterLocation("testLoc3");
            string testitemName = "TestSomething2";
            int InitialAamount = l.CheckStock(testitemName);
            Assert.Equal(0, InitialAamount); //assume there is zero of nothing initially
            StoreCatalogue.Instance.RegisterItem(testitemName, 50);

            int currentAmount = l.AddInventory(testitemName, 5); ;

            //act
            int checkCurrentAmmount = l.CheckStock(testitemName);

            //assert
            Assert.Equal(currentAmount, checkCurrentAmmount);
        }   
    }
}
