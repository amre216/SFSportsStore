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

        private IProductsRepository _prodRepo;
        private int _pageSize = 4;

        //On init of product controller accept product interface - resolves to mock list of products
        public ProductController(IProductsRepository prodRepoParam, int pageSize = 3)
        {
            this._prodRepo = prodRepoParam;
            this._pageSize = pageSize;
        }

        public ViewResult List(int page = 1)
        {
            //Init pageview model with product list and pagination settings
            ProductListViewModel listViewPageModel = new ProductListViewModel
            {
                Products = _prodRepo.Products.OrderBy(p => p.ProductId).Skip((page - 1) * _pageSize).Take(_pageSize),
                PagingInfo = new PagingInfo { CurrentPage = page, ItemsPerPage = _pageSize, TotalItems = _prodRepo.Products.Count() }
            };

            return View(listViewPageModel);
        }

    }
}