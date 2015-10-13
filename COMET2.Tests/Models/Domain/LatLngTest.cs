using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using COMET.Model.Domain;
using System.Collections.Generic;

namespace COMET.Tests.Models.Domain {
    [TestClass()]
    public class LatLng_Test {
        private decimal[] latitude =    { 0, 90, -90 };
        private decimal[] longitude =   { 0, 180, -180 };
        private IList<LatLng> goodLLDec = new List<LatLng>();
        private IList<LatLng> goodLLStr = new List<LatLng>();

        [TestInitialize()]
        public void setup() {
            for (int i = 0; i < latitude.Length; i++) {
                goodLLDec.Add(new LatLng(latitude[i], longitude[i]));
                goodLLStr.Add(new LatLng(longitude[i] + "," + latitude[i]));
            }
        }

        [TestMethod]
        public void TestLatitudeDec() {
            for (int i = 0; i < goodLLDec.Count; i++)
                Assert.AreEqual(latitude[i], goodLLDec[i].Lat);
        }

        [TestMethod]
        public void TestLatitudeStr() {
            for (int i = 0; i < goodLLStr.Count; i++)
                Assert.AreEqual(latitude[i], goodLLStr[i].Lat);
        }

        [TestMethod]
        public void TestLongitudeDec() {
            for (int i = 0; i < goodLLDec.Count; i++)
                Assert.AreEqual(longitude[i], goodLLDec[i].Lng);
        }

        [TestMethod]
        public void TestLongitudeStr() {
            for (int i = 0; i < goodLLStr.Count; i++)
                Assert.AreEqual(longitude[i], goodLLStr[i].Lng);
        }

        [TestMethod]
        public void TestForFails() {
            try {
                new LatLng((decimal)-90.000001, (decimal)0);
                Assert.Fail("Format Exception should have been thrown for latitude <-90.");
            } catch (FormatException) { }

            try {
                new LatLng((decimal)90.000001, (decimal)0);
                Assert.Fail("Format Exception should have been thrown for latitude >90.");
            } catch (FormatException) { }

            try {
                new LatLng((decimal)0, (decimal)-180.000001);
                Assert.Fail("Format Exception should have been thrown for longitude <-180.");
            } catch (FormatException) { }

            try {
                new LatLng((decimal)0, (decimal)180.000000001);
                Assert.Fail("Format Exception should have been thrown for longitude >180.");
            } catch (FormatException) { }
        }


    }
}
