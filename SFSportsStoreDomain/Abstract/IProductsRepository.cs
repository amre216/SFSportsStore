using System.Collections.Generic;
using SFSportsStore.Domain.Entities;

namespace SFSportsStore.Domain.Abstract
{
    public interface IProductsRepository
    {
        //Interface for implementing a repository / collection of Products
        IEnumerable<Product> Products { get; }

        //Save product
        void SaveProduct(Product prod);

        //Delete product
        void DeleteProduct(Product prod);
    }
}
