using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BookCollection.DAL;
using System.Collections.Generic;
using BookCollection.Logging;

namespace BookCollection.Tests.DAL
{
    [TestClass]
    public class InitiazerTest
    {

        [TestMethod]
        public void TestMethod1()
        {
            var logger = new TraceLogger();
            var dataProvider = new MockDataProvider();

        }
    }

    public class MockDataProvider : ISeedDataProvider
    {
        public IEnumerable<seedDataModel> GetData()
        {
            throw new NotImplementedException();
        }
    }
}
