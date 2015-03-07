using System.Collections.Generic;
using SFSportsStore.Domain.Abstract;
using SFSportsStore.Domain.Entities;

namespace SFSportsStore.Domain.Concrete
{
    public class EFProductsRepository : IProductsRepository
    {
        //Instance of database cotnext - Product <- Products table
        private EFDBContext _dbContext = new EFDBContext();

        public IEnumerable<Product> Products { get { return _dbContext.Products; } }
    }
}
