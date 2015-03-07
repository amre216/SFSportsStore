using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SFSportsStore.Domain.Abstract;
using SFSportsStore.Domain.Entities;
using SFSportsStore.WebUI.Models;

namespace SFSportsStore.WebUI.Controllers
{
    public class ProductController : Controller
    {
        private IProductsRepository _products;
        private PagingInfo _pageingInfo;
        private ProductListViewModel _prodListViewModel;
        private int _pageSize = 3;

        public ProductController(IProductsRepository prodRepo)
        {
            _products = prodRepo;
            
        }

        public ViewResult List(string currentCategory, int page = 1)
        {
            _pageingInfo = new PagingInfo { CurrentPage = page, ItemsPerPage = _pageSize, TotalItems = _products.Products.Where(p => currentCategory == null || currentCategory.ToLower() == p.Category.ToLower()).Count() };

            _prodListViewModel = new ProductListViewModel { 
                PagingInfo = _pageingInfo,
                Products = _products.Products.Where(p => currentCategory == null || currentCategory.ToLower() == p.Category.ToLower()).OrderBy(p => p.ProductId).Skip(page - 1).Take(_pageSize), 
                CurrentCategory = currentCategory 
            };

            return View(_prodListViewModel);
        }

    }
}