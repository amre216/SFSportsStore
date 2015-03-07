using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SFSportsStore.WebUI.Models
{
    public class CategoryMenuViewModel
    {
        public IEnumerable<string> categories { get; set; }
        public string currentCategory;
    }
}