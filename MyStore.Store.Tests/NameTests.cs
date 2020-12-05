using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using MyStore.Store;

namespace MyStore.Store.Tests
{
    public class NameTests
    {

        [Theory]
        [InlineData("Someone T anyone","Someone", 'T',"anyone")]
        [InlineData("          Someone T        anyone", "Someone", 'T', "anyone")]
        public void RegisterLocation(String tostrName, string first, char? middle, string last)
        {
            //Arrange
            Name othername = new Name(first, last, middle);

            //Act
            Name testname = new Name(tostrName);
            //Assert
            Assert.Equal(first, testname.First);

            Assert.Equal(middle, testname.MiddleInitial);

            Assert.Equal(last, testname.Last);

            Assert.Equal(othername, testname);
        }
    }
}
