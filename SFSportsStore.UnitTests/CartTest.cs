using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using SFSportsStore.Domain.Entities;


namespace SFSportsStore.UnitTests
{
    [TestClass]
    public class CartTest
    {

        private Cart _testCart = new Cart();

        private Product[] _testProds = new Product[] {
                new Product { Name = "TProd1", Price = 10, ProductId = 1 },
                new Product { Name = "TProd2", Price = 20, ProductId = 2 },
                new Product { Name = "TProd3", Price = 30, ProductId = 3 }
            };

        [TestMethod]
        public void Can_Add_Items()
        {
            _testCart.AddItem(_testProds[0], 2);
            _testCart.AddItem(_testProds[1], 1);

            CartLine[] testCartLines = _testCart.Lines.ToArray();

            Assert.AreEqual(testCartLines[0].Product.Name, "TProd1");
            Assert.AreEqual(testCartLines[0].Quantity, 2);
            Assert.AreEqual(testCartLines[1].Product.Name, "TProd2");
            Assert.AreEqual(testCartLines[1].Quantity, 1);
        }

        [TestMethod]
        public void Can_Remove_Items()
        {
            _testCart.AddItem(_testProds[0], 2);
            _testCart.AddItem(_testProds[0], 2);
            _testCart.AddItem(_testProds[1], 1);

            _testCart.RemoveLine(_testProds[0]);

            CartLine[] testCartLines = _testCart.Lines.ToArray();


            Assert.AreEqual(testCartLines[0].Product.Name, "TProd2");
            Assert.AreEqual(testCartLines[0].Quantity, 1);
        }

        [TestMethod]
        public void Can_Compute_TotalValue()
        {
            _testCart.AddItem(_testProds[0], 2);
            _testCart.AddItem(_testProds[0], 2);
            _testCart.AddItem(_testProds[1], 1);

            decimal totalValExpected = 60;
            decimal totalValComputed = _testCart.ComputeTotalValue();

            Assert.AreEqual(totalValExpected, totalValComputed);
        }

        [TestMethod]
        public void Can_Remove_AllItems()
        {
            _testCart.AddItem(_testProds[0], 2);
            _testCart.AddItem(_testProds[0], 2);
            _testCart.AddItem(_testProds[1], 1);

            _testCart.Clear();

            CartLine[] testCartLines = _testCart.Lines.ToArray();

            Assert.AreEqual(_testCart.Lines.Count(), 0);
        }
    }
}
