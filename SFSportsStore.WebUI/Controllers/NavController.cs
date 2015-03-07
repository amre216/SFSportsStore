using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SFSportsStore.Domain.Abstract;
using SFSportsStore.Domain.Concrete;
using SFSportsStore.Domain.Entities;
using SFSportsStore.WebUI.Models;

namespace SFSportsStore.WebUI.Controllers
{
    public class NavController : Controller
    {
        private IProductsRepository _catRepo;
        private CategoryMenuViewModel _catMenuViewModel = new CategoryMenuViewModel();

        public NavController(IProductsRepository prodRepoResolver)
        {
            _catRepo = prodRepoResolver;
        }

        public PartialViewResult CategoryMenu(string currentCategory = null)
        {
            _catMenuViewModel.categories = _catRepo.Products.Select(x => x.Category).Distinct().OrderBy(x => x);
            _catMenuViewModel.currentCategory = currentCategory;

            return PartialView(_catMenuViewModel);
        }
    }
}