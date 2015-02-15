using System.Collections.Generic;
using SFSportsStore.Domain.Entities;
using SFSportsStore.Domain.Abstract;

namespace SFSportsStore.Domain.Concrete
{
    public class EFProductRepository : IProductsRepository
    {
        //Use our database context - where Product object represent rowns in the Product table
        private EFDbContext dbContext = new EFDbContext();

        private Product _prod;

        public EFProductRepository(Product prodParam) {
            _prod = prodParam;
        }

        public IEnumerable<Product> Products { get { return dbContext.Products; } set { dbContext.Products.Add(_prod); } }
    }
}
