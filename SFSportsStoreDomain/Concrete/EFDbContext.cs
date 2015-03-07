using System.Collections.Generic;
using System.Data.Entity;
using SFSportsStore.Domain.Entities;


namespace SFSportsStore.Domain.Concrete
{
    public class EFDBContext : DbContext
    {
        //Database context - relationship between Products table in context resolving each row as a Product obj.
        //Property is a databaset (enumerable collection) of product objects - Type is object resolved to from Table (property name)
        public DbSet<Product> Products { get; set; }

        public DbSet<Category> TestCategories { get; set; }
    }
}
