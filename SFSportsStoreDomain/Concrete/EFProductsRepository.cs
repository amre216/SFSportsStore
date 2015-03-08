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

        public void SaveProduct(Product prod) {
            
            //If new products
            if(prod.ProductId == 0)
            {
                _dbContext.Products.Add(prod);
            }
            else
            {
                //Find the prod in db
                Product dbProd = _dbContext.Products.Find(prod.ProductId);
                if (dbProd != null)
                {
                    //Update the prod with prod data passed
                    dbProd.Name = prod.Name;
                    dbProd.Description = prod.Description;
                    dbProd.Price = prod.Price;
                    dbProd.Category = prod.Category;
                }
            }

            //Commit changes to db
            _dbContext.SaveChanges();

        }

        public void DeleteProduct(Product prod)
        {
            Product dbProd = _dbContext.Products.Find(prod.ProductId);

            if (dbProd != null)
            {
                _dbContext.Products.Remove(dbProd);
                _dbContext.SaveChanges();
            }
        }
    }
}
