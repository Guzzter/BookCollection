using BookCollection.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookCollection.Tests.Helpers
{
    [TestClass]
    public class ConvertersTest
    {
        [TestMethod]
        public void TestRemovalOfSerieNr()
        {
            Assert.AreEqual("", Converters.RenoveSerieNr(null));
            Assert.AreEqual("", Converters.RenoveSerieNr(""));
            Assert.AreEqual("Serie XYZ", Converters.RenoveSerieNr("Serie XYZ (1)"));

        }
    }
}
