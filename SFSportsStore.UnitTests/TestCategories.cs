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
    /// Summary description for TestCategories
    /// </summary>
    [TestClass]
    public class TestCategories
    {

        NavController _navContrl = new NavController((IProductsRepository)null);
        Mock<IProductsRepository> _mockProdRepo = new Mock<IProductsRepository>();
        
        public TestCategories()
        {
            _mockProdRepo.Setup(p => p.Products).Returns(new Product[] {
                new Product { ProductId = 1, Name = "P1", Category = "Apples"},
                new Product { ProductId = 2, Name = "P2", Category = "Apples"},
                new Product { ProductId = 3, Name = "P3", Category = "Plums"},
                new Product { ProductId = 4, Name = "P4", Category = "Oranges"}, 
            });

            _navContrl = new NavController(_mockProdRepo.Object);
        }

        [TestMethod]
        public void Can_Create_Categories()
        {

            //Act - Get categories from mock repo using NavController
            string[] resultCats = ((IEnumerable<string>)_navContrl.CategoryMenu().Model).ToArray();

            //Assert 
            Assert.IsTrue(resultCats.Length == 3);
            Assert.IsTrue(resultCats[0] == "Apples");
            Assert.IsTrue(resultCats[1] == "Oranges");
            Assert.IsTrue(resultCats[2] == "Plums");
        }

        [TestMethod]
        public void Can_Indicate_CurrentCategry()
        {
            //Arrage - Set category to selct
            string currentSetCat = "Apples";

            CategoryMenuViewModel catMenuViewModel = (CategoryMenuViewModel)_navContrl.CategoryMenu(currentSetCat).Model;

            Assert.AreEqual(currentSetCat, catMenuViewModel.currentCategory);
        }
    }
}
