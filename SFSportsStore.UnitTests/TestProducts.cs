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
            ProductController prodContrl = new ProductController(prodRepoMock.Object, 3);
            
            //Act
            //Create enumerable list of products - getting page 2
            ProductListViewModel result = (ProductListViewModel)prodContrl.List(2).Model; 

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
            ProductController prodCtrl = new ProductController(mockProdRepo.Object, 3);

            //Act - get product list as ProductListViewModel
            ProductListViewModel prodAndPagingViewModel = (ProductListViewModel)prodCtrl.List(2).Model; 

            //Assert
            Assert.AreEqual(prodAndPagingViewModel.PagingInfo.CurrentPage, 2);
            Assert.AreEqual(prodAndPagingViewModel.PagingInfo.ItemsPerPage, 3);
            Assert.AreEqual(prodAndPagingViewModel.PagingInfo.TotalItems, 5);
            Assert.AreEqual(prodAndPagingViewModel.PagingInfo.TotalPages, 2);
        }
    }

}
