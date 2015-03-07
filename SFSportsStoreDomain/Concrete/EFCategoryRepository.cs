using System.Collections.Generic;
using SFSportsStore.Domain.Abstract;
using SFSportsStore.Domain.Entities;


namespace SFSportsStore.Domain.Concrete
{
    public class EFCategoryRepository : ICategoryRepository
    {
        private EFDBContext _dbContext = new EFDBContext();

        public IEnumerable<Category> TestCategories { get { return _dbContext.TestCategories; } }
    }
}
