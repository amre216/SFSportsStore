using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using SFSportsStore.Domain.Entities;
using Moq;
using SFSportsStore.Domain.Abstract;
using SFSportsStore.WebUI.Controllers;
using SFSportsStore.WebUI.Models;
using System.Web.Mvc;


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

        //Arrange - Create mock repo, create cart, create cart controller
        private Mock<IProductsRepository> _mockProdRepo = new Mock<IProductsRepository>();

        CartController _cartCtrl;

        public CartTest()
        {
            _mockProdRepo.Setup(p => p.Products).Returns(new Product[] {
                new Product { Category = "Apples", Name = "P1", ProductId = 1 }
            }.AsQueryable());

            _cartCtrl = new CartController(_mockProdRepo.Object, null);
        }



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

        [TestMethod]
        public void Can_Add_ToCart_v2()
        {
            //Act
            _cartCtrl.AddToCart(1, null, _testCart);

            //Assert
            Assert.AreEqual(_testCart.Lines.Count(), 1);
            Assert.AreEqual(_testCart.Lines.ToArray()[0].Product.ProductId, 1);
        }

        [TestMethod]
        public void Cart_Can_Go_To_CartScreen()
        {
            //Act
            RedirectToRouteResult redrResult = _cartCtrl.AddToCart(1, "TestUrl", _testCart);

            //Assert
            Assert.AreEqual(redrResult.RouteValues["action"], "Index");
            Assert.AreEqual(redrResult.RouteValues["returnUrl"], "TestUrl");
        }

        [TestMethod] 
        public void Cart_Can_View_Contents()
        {
            Cart compareToCart = new Cart();
            _cartCtrl.AddToCart(1, "TestUrl", compareToCart);

            _cartCtrl.AddToCart(1, "TestUrl", _testCart);

            CartIndexViewModel cartViewModel = (CartIndexViewModel)_cartCtrl.Index("TestUrl", _testCart).ViewData.Model;

            Assert.AreSame(compareToCart, cartViewModel.Cart);
        }

        [TestMethod]
        public void Cart_Content_IsValid()
        {
            Cart compareToCart = new Cart();
            _cartCtrl.AddToCart(1, "TestUrl", compareToCart);
            _cartCtrl.AddToCart(1, "TestUrl", compareToCart);
            _cartCtrl.AddToCart(1, "TestUrl", compareToCart);

            _cartCtrl.AddToCart(1, "TestUrl", _testCart);
            _cartCtrl.AddToCart(1, "TestUrl", _testCart);
            _cartCtrl.AddToCart(1, "TestUrl", _testCart);

            CartIndexViewModel cartViewModel = (CartIndexViewModel)_cartCtrl.Index("TestUrl", _testCart).ViewData.Model;

            Assert.AreEqual(compareToCart.ToString(), cartViewModel.Cart.ToString());
        }

        [TestMethod]
        public void Cannot_Checkout_Empty_Cart()
        {
            //Arrange - Empty cart, new order proc, empty ship details new controller with order proc
            Cart emptyCart = new Cart();
            Mock<IOrderProcessor> mockOrderProc = new Mock<IOrderProcessor>();
            ShippingDetails shipDetails = new ShippingDetails();
            CartController crtCtrlWithOrderProc = new CartController(null, mockOrderProc.Object);

            //Act - Submit empty cart with empty shipping details
            ViewResult ctrlActionResult = crtCtrlWithOrderProc.Checkout(emptyCart, shipDetails);

            //Assert
            //Check that the order has not been passed by the processor
            mockOrderProc.Verify(m => m.ProcessOrder(It.IsAny<Cart>(), It.IsAny<ShippingDetails>()), Times.Never());

            //Check that the method is returning the default view
            Assert.AreEqual("", ctrlActionResult.ViewName);

            //Check that the model state is invalid
            Assert.AreEqual(false, ctrlActionResult.ViewData.ModelState.IsValid);
        }

        [TestMethod]
        public void Cannot_Checkout_With_Invalid_Shipping()
        {
            Mock<IOrderProcessor> mockOrderProc = new Mock<IOrderProcessor>();
            CartController crtCtrlWithOrderProc = new CartController(null, mockOrderProc.Object);
            ShippingDetails shipDetails = new ShippingDetails();

            //Arrange - set error in the model
            crtCtrlWithOrderProc.ModelState.AddModelError("error", "invalid shipping");

            //Act attempt to get result
            ViewResult ctrlActionResult = crtCtrlWithOrderProc.Checkout(_testCart, shipDetails);

            //Assert
            //Check that the order has not been passed by the processor
            mockOrderProc.Verify(m => m.ProcessOrder(It.IsAny<Cart>(), It.IsAny<ShippingDetails>()), Times.Never());

            //Check that the method is returning the default view
            Assert.AreEqual("", ctrlActionResult.ViewName);

            //Check that the model state is invalid
            Assert.AreEqual(false, ctrlActionResult.ViewData.ModelState.IsValid);
        }

        [TestMethod]
        public void Can_Checkout_Valid_Cart_And_Shipping()
        {
            //Arrange
            Mock<IOrderProcessor> mockOrderProc = new Mock<IOrderProcessor>();
            CartController crtCtrlWithOrderProc = new CartController(null, mockOrderProc.Object);
            ShippingDetails shipDetails = new ShippingDetails();

            //Add prods to cart
            _testCart.AddItem(_testProds[0], 2);
            _testCart.AddItem(_testProds[1], 1);

            //Act - Attempt checkout with valid cart and empty shipping details
            ViewResult ctrlActionResult = crtCtrlWithOrderProc.Checkout(_testCart, shipDetails);

            //Assert
            //Check that order processor has been exec once
            mockOrderProc.Verify(m => m.ProcessOrder(It.IsAny<Cart>(), It.IsAny<ShippingDetails>()), Times.Once());

            //Check that we get back the completed view
            Assert.AreEqual("Completed", ctrlActionResult.ViewName);

            //Check that the viewstate is valid
            Assert.AreEqual(true, ctrlActionResult.ViewData.ModelState.IsValid);
        }
    }
}
