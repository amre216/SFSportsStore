using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using SFSportsStore.Domain.Abstract;
using SFSportsStore.Domain.Entities;
using SFSportsStore.WebUI.Controllers;
using SFSportsStore.WebUI.Models;
using SFSportsStore.WebUI.HtmlHelpers;
using Moq;
namespace SFSportsStore.UnitTests
{
    /// <summary>
    /// Summary description for MockTesting
    /// </summary>
    [TestClass]
    public class MockTesting
    {

        Mock<IProductsRepository> _prodMock;
        PagingInfo _pagingInfo;
        Func<int, string> _pageUrl;

        public MockTesting()
        {
            //Constructor: Create product mocking object
            _prodMock = new Mock<IProductsRepository>();

            _prodMock.Setup(p => p.Products).Returns(new Product[] { 
                new Product { Name = "P1", ProductId = 1 },
                new Product { Name = "P2", ProductId = 2 },
                new Product { Name = "P3", ProductId = 3 },
                new Product { Name = "P4", ProductId = 4 },
                new Product { Name = "P5", ProductId = 5 }
            });

            _pagingInfo = new PagingInfo { CurrentPage = 2, ItemsPerPage = 2, TotalItems = _prodMock.Object.Products.Count() };

            _pageUrl = i => "Page" + i;
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void Product_Can_Paginate()
        {
            //Arrange
            ProductController prodCtrl = new ProductController(_prodMock.Object, 3);

            //Act - Get product list and pagination view model
            ProductListViewModel prodListPageModel = (ProductListViewModel)prodCtrl.List(2).Model;

           //Assert 
           Product[] prods = prodListPageModel.Products.ToArray();
           Assert.IsTrue(prods.Length == 2);
           Assert.AreEqual(prods[0].Name, "P4");
           Assert.AreEqual(prods[1].Name, "P5");
        }

        [TestMethod]
        public void HtmlHelperPaginate_Can_Generate_Page_Links()
        {
            //Arrange - define a html helper - we need one as we are exntening it
            HtmlHelper htmlHelper = null;

            //Act
            MvcHtmlString htmlString = htmlHelper.PageLinks(_pagingInfo, _pageUrl);

            Assert.AreEqual(@"<a class=""btn btn-default"" href=""Page1"">1</a><a class=""btn btn-default btn-primary selected"" href=""Page2"">2</a><a class=""btn btn-default"" href=""Page3"">3</a>", htmlString.ToString());
        }

        [TestMethod]
        public void Can_Send_ProductAndPaging_ViewModel()
        {
            //Arrange 
            ProductController prodCtrl = new ProductController(_prodMock.Object, 3);

            //Act
            ProductListViewModel prodListPageModel = (ProductListViewModel)prodCtrl.List(2).Model;

            //Assert
            Assert.AreEqual(prodListPageModel.PagingInfo.CurrentPage, 2);
            Assert.AreEqual(prodListPageModel.PagingInfo.ItemsPerPage, 3);
            Assert.AreEqual(prodListPageModel.PagingInfo.TotalItems, 5);
            Assert.AreEqual(prodListPageModel.PagingInfo.TotalPages, 2);
        }
    }
}
