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
    [TestClass]
    public class TestProducts
    {
        [TestMethod]
        public void Product_Can_Paginate()
        {
            //Arrange 
            Mock<IProductsRepository> prodRepoMock = new Mock<IProductsRepository>();

            prodRepoMock.Setup(m => m.Products).Returns(new Product[] { 
                new Product { Name = "P1", ProductId = 1 },
                new Product { Name = "P2", ProductId = 2 },
                new Product { Name = "P3", ProductId = 3 },
                new Product { Name = "P4", ProductId = 4 },
                new Product { Name = "P5", ProductId = 5 }
            });

            //New product controller with our mock repo of products
            ProductController prodContrl = new ProductController(prodRepoMock.Object);

            //Act
            //Create enumerable list of products - getting page 2
            ProductListViewModel result = (ProductListViewModel)prodContrl.List(null, 2).Model;

            //Assert
            Product[] prodArr = result.Products.ToArray();
            Assert.IsTrue(prodArr.Length == 2);
            Assert.AreEqual(prodArr[0].Name, "P4");
            Assert.AreEqual(prodArr[1].Name, "P5");
        }

        [TestMethod]
        public void HtmlHelperPaginate_Can_Generate_Page_Links()
        {
            //Arrange - Define a heml helper -> assign extention method
            HtmlHelper testHelper = null;

            //Arrange - create paging date;
            PagingInfo pagingInfo = new PagingInfo { CurrentPage = 2, TotalItems = 28, ItemsPerPage = 10 };

            //Arrange - setup the delegate using lambada exp 
            Func<int, string> pageUrl = i => "Page" + i;

            //Act
            MvcHtmlString paginatorHtml = testHelper.PageLinks(pagingInfo, pageUrl);

            //Assert
            Assert.AreEqual(@"<a class=""btn btn-default"" href=""Page1"">1</a><a class=""btn btn-default btn-primary selected"" href=""Page2"">2</a><a class=""btn btn-default"" href=""Page3"">3</a>", paginatorHtml.ToString());
        }

        [TestMethod]
        public void Can_Send_ProductAndPaging_ViewModel()
        {
            //Arrange
            Mock<IProductsRepository> mockProdRepo = new Mock<IProductsRepository>();

            mockProdRepo.Setup(m => m.Products).Returns(new Product[]
            {
                new Product { Name = "P1", ProductId = 1 },
                new Product { Name = "P2", ProductId = 2 },
                new Product { Name = "P3", ProductId = 3 },
                new Product { Name = "P4", ProductId = 4 },
                new Product { Name = "P5", ProductId = 5 }
            });

            //Arrange 
            ProductController prodCtrl = new ProductController(mockProdRepo.Object);

            //Act - get product list as ProductListViewModel
            ProductListViewModel prodAndPagingViewModel = (ProductListViewModel)prodCtrl.List(null, 2).Model;

            //Assert
            Assert.AreEqual(prodAndPagingViewModel.PagingInfo.CurrentPage, 2);
            Assert.AreEqual(prodAndPagingViewModel.PagingInfo.ItemsPerPage, 3);
            Assert.AreEqual(prodAndPagingViewModel.PagingInfo.TotalItems, 5);
            Assert.AreEqual(prodAndPagingViewModel.PagingInfo.TotalPages, 2);
        }

        [TestMethod]
        public void Can_Filter_Categories()
        {
            //Arrange - Create mock product repo.
            Mock<IProductsRepository> mockRepo = new Mock<IProductsRepository>();

            mockRepo.Setup(m => m.Products).Returns(new Product[] {
                new Product { Category = "TestCat1", Name = "Cat1Obj1" },
                new Product { Category = "TestCat1", Name = "Cat1Obj2" },
                new Product { Category = "TestCat2", Name = "Cat2Obj1" },
                new Product { Category = "TestCat3", Name = "Cat3Obj1" }
            });

            //Init product controller
            ProductController prodControl = new ProductController(mockRepo.Object);
            
            //Act
            Product[] resultTestCatNone = ((ProductListViewModel)prodControl.List(null, 1).Model).Products.ToArray();
            Product[] resultTestCat1 = ((ProductListViewModel)prodControl.List("TestCat1", 1).Model).Products.ToArray();
            Product[] resultTestCat3 = ((ProductListViewModel)prodControl.List("TestCat3", 1).Model).Products.ToArray();

            //Assert
            Assert.IsTrue(resultTestCatNone.Length == 4);
            Assert.IsTrue(resultTestCatNone[0].Name == "Cat1Obj1");
            Assert.IsTrue(resultTestCatNone[1].Name == "Cat1Obj2");
            Assert.IsTrue(resultTestCat1.Length == 2);
            Assert.IsTrue(resultTestCat1[0].Name == "Cat1Obj1");
            Assert.IsTrue(resultTestCat1[1].Name == "Cat1Obj2");
            Assert.IsTrue(resultTestCat3.Length == 1);
            Assert.IsTrue(resultTestCat3[0].Name == "Cat3Obj1");
            
        }
    }

}
