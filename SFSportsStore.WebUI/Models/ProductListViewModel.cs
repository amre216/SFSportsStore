using System.Collections.Generic;
using SFSportsStore.Domain.Entities;

namespace SFSportsStore.WebUI.Models
{
    public class ProductListViewModel
    {
        public IEnumerable<Product> Products { get; set; }
        public PagingInfo PagingInfo { get; set; }
        public string CurrentCategory { get; set; }
    }
}