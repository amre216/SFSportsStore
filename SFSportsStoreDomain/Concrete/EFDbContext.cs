using SFSportsStore.Domain.Entities;
using System.Data.Entity;

namespace SFSportsStore.Domain.Concrete
{
    //Creates a database context class
    public class EFDbContext : DbContext
    {
        //Creates a binding in this context where Products objects represent rows in the products table
        public DbSet<Product> Products { get; set; }
    }

}
