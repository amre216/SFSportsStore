using System.Collections.Generic;
using SFSportsStore.Domain.Entities;

namespace SFSportsStore.Domain.Abstract
{
    public interface IProductsRepository
    {
        IEnumerable<Product> Products { get; }
    }
}
