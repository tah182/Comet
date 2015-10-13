using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using COMET.Model.Console.Domain.View;

namespace COMET.Tests.Models.Domain.Console {
    [TestClass]
    public class ElementViewTest {
        [TestMethod]
        public void NewElementViewTest() {
            ElementView elementView = new ElementView();
        }
    }
}
