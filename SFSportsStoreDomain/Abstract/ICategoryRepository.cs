using System.Collections.Generic;
using SFSportsStore.Domain.Entities;


namespace SFSportsStore.Domain.Abstract
{
    public interface ICategoryRepository
    {
        IEnumerable<Category> TestCategories { get; }
    }
}
