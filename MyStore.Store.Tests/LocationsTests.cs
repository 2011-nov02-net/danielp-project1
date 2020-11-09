using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using MyStore.Store;
using System.Diagnostics;

namespace MyStore.Store.Tests
{
    public class LocationsTests
    {

        [Fact]
        public void RegisterLocation()
        {
            //Arrange
            string newlocname = "TestLocation0";
            Assert.True(LocationDoesntExistYet(newlocname));
            //Act
            Location newloc = Locations.Instance.RegisterLocation(newlocname);
            //Assert
            Assert.NotNull(newloc);
            Assert.Equal(newlocname, newloc.Where);
        }


        [Fact]
        public void GetRealLocation()
        {
            //arrange
            string locname = "TestLocation1";
            Assert.True(LocationDoesntExistYet(locname));
            Location mylocation = Locations.Instance.RegisterLocation(locname);
            //act
            Location returnedLoc = Locations.Instance.GetLocation(locname);

            //assert
            Assert.Equal(mylocation, returnedLoc);
        }

        [Fact]
        public void FailGetFakeLocation()
        {
            //arrange
            string locname = "TestLocation2";
            Assert.True(LocationDoesntExistYet(locname));

            //act
            //assert
            Assert.ThrowsAny<ArgumentException>(() => Locations.Instance.GetLocation(locname));

        }


        private Boolean LocationDoesntExistYet(string locname)
        {
            Boolean doesnotexist = true;
            foreach (Location loc in Locations.Instance.Stores)
            {
                if (loc.Where == locname)
                {
                    doesnotexist = false;
                }
            }

            return doesnotexist;
        }
    }
}
