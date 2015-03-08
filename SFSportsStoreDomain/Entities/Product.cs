using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace SFSportsStore.Domain.Entities
{
    public class Product
    {
        [HiddenInput(DisplayValue = false)]
        public int ProductId { get; set; }
        [DataType(DataType.MultilineText)]
        public string Name { get; set; }
        public string Description { get; set; }
        public Decimal Price { get; set; }
        public string Category { get; set; }
    }
}
