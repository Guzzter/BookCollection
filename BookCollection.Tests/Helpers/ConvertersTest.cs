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
        public void ExtractSerieNr()
        {
            Assert.AreEqual("I", Converters.ExtractSerieNr("British Monarchs Series I"));
            Assert.AreEqual("II", Converters.ExtractSerieNr("British Monarchs Series II"));
            Assert.AreEqual("III", Converters.ExtractSerieNr("British Monarchs Series III"));
            Assert.AreEqual("IV", Converters.ExtractSerieNr("British Monarchs Series IV"));
            Assert.AreEqual("V", Converters.ExtractSerieNr("British Monarchs Series V"));
            Assert.AreEqual("VI", Converters.ExtractSerieNr("British Monarchs Series VI"));
            Assert.AreEqual("VII", Converters.ExtractSerieNr("British Monarchs Series VII"));
            Assert.AreEqual("VIII", Converters.ExtractSerieNr("British Monarchs Series VIII"));
            Assert.AreEqual("IX", Converters.ExtractSerieNr("British Monarchs Series IX"));
            Assert.AreEqual("X", Converters.ExtractSerieNr("British Monarchs Series X"));
            Assert.AreEqual("XI", Converters.ExtractSerieNr("British Monarchs Series XI"));


            Assert.AreEqual("1", Converters.ExtractSerieNr("Serie XYZ (1)"));
            Assert.AreEqual("1", Converters.ExtractSerieNr("Serie XYZ 1"));
            Assert.AreEqual("18", Converters.ExtractSerieNr("Serie XYZ 18"));
            Assert.AreEqual("122", Converters.ExtractSerieNr("Serie XYZ 122"));
            Assert.AreEqual("II", Converters.ExtractSerieNr("Serie XYZ (II)"));

            var testA = Converters.ExtractSerieNr("Serie XYZ (II), 2e druk");
            Assert.AreEqual("II 2e druk", testA);
            Assert.AreEqual("III 3e druk", Converters.ExtractSerieNr("Serie XYZ (III), 3e druk"));

            Assert.AreEqual("II", Converters.ExtractSerieNr("Serie XYZ (II) / LHR"));
            Assert.AreEqual("IIIA", Converters.ExtractSerieNr("Serie XYZ IIIA"));
            Assert.AreEqual("IIIB", Converters.ExtractSerieNr("Serie XYZ IIIB"));
            Assert.AreEqual("deel 1", Converters.ExtractSerieNr("Serie XYZ - deel 1"));
            Assert.AreEqual("1500 t/m 1600", Converters.ExtractSerieNr("Serie XYZ 1500-1600"));

        }

        [TestMethod]
        public void TestRemovalOfSerieNr()
        {
            Assert.AreEqual("", Converters.RemoveSerieNr(null));
            Assert.AreEqual("", Converters.RemoveSerieNr(""));
            Assert.AreEqual("British Monarchs Series", Converters.RemoveSerieNr("British Monarchs Series I"));
            Assert.AreEqual("British Monarchs Series", Converters.RemoveSerieNr("British Monarchs Series II"));
            Assert.AreEqual("British Monarchs Series", Converters.RemoveSerieNr("British Monarchs Series III"));
            Assert.AreEqual("British Monarchs Series", Converters.RemoveSerieNr("British Monarchs Series IV"));
            Assert.AreEqual("British Monarchs Series", Converters.RemoveSerieNr("British Monarchs Series V"));
            Assert.AreEqual("British Monarchs Series", Converters.RemoveSerieNr("British Monarchs Series VI"));
            Assert.AreEqual("British Monarchs Series", Converters.RemoveSerieNr("British Monarchs Series VII"));
            Assert.AreEqual("British Monarchs Series", Converters.RemoveSerieNr("British Monarchs Series VIII"));
            Assert.AreEqual("British Monarchs Series", Converters.RemoveSerieNr("British Monarchs Series IX"));
            Assert.AreEqual("British Monarchs Series", Converters.RemoveSerieNr("British Monarchs Series X"));
            Assert.AreEqual("British Monarchs Series", Converters.RemoveSerieNr("British Monarchs Series XI"));

            
            Assert.AreEqual("Serie XYZ", Converters.RemoveSerieNr("Serie XYZ (1)"));
            Assert.AreEqual("Serie XYZ", Converters.RemoveSerieNr("Serie XYZ 1"));
            Assert.AreEqual("Serie XYZ", Converters.RemoveSerieNr("Serie XYZ 18"));
            Assert.AreEqual("Serie XYZ", Converters.RemoveSerieNr("Serie XYZ 122"));
            Assert.AreEqual("Serie XYZ", Converters.RemoveSerieNr("Serie XYZ (II)"));
            Assert.AreEqual("Serie XYZ", Converters.RemoveSerieNr("Serie XYZ (II), 2e druk"));
            Assert.AreEqual("Serie XYZ", Converters.RemoveSerieNr("Serie XYZ (II), 3e druk"));
            
            Assert.AreEqual("Serie XYZ", Converters.RemoveSerieNr("Serie XYZ (II) / LHR"));
            Assert.AreEqual("Serie XYZ", Converters.RemoveSerieNr("Serie XYZ IIIA"));
            Assert.AreEqual("Serie XYZ", Converters.RemoveSerieNr("Serie XYZ IIIB"));
            Assert.AreEqual("Serie XYZ", Converters.RemoveSerieNr("Serie XYZ - deel 1"));
            Assert.AreEqual("Serie XYZ", Converters.RemoveSerieNr("Serie XYZ 1500-1600"));



        }
    }
}
