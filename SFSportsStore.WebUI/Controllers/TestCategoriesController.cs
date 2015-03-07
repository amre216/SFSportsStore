using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SFSportsStore.Domain.Concrete;
using SFSportsStore.Domain.Entities;
using SFSportsStore.Domain.Abstract;

namespace SFSportsStore.WebUI.Controllers
{
    public class TestCategoriesController : Controller
    {
        IEnumerable<Category> _categories;

        public TestCategoriesController(ICategoryRepository catRepo)
        {
            _categories = catRepo.TestCategories;
        }
        // GET: TestCategories
        public ActionResult ListCategories()
        {
            return View(_categories);
        }
    }
}