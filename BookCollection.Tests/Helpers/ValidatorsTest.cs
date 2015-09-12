using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BookCollection.Helpers;

namespace BookCollection.Tests.Helpers
{
    [TestClass]
    public class ValidatorsTest
    {
        [TestMethod]
        public void IsValidIsbnTest()
        {
            Assert.IsFalse(Validators.IsValidIsbn(""));

            Assert.IsFalse(Validators.IsValidIsbn("1901259099"));
            Assert.IsTrue(Validators.IsValidIsbn("1-901259-09-9"));
            Assert.IsTrue(Validators.IsValidIsbn("ISBN 1-901259-09-9"));
        }
    }
}
