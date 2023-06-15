using Microsoft.EntityFrameworkCore;
using System;

namespace HelloWorld
{
    // Database context represents a session with the database and can be used to queuery and save instances
    public class ProductDatabase : DbContext
    {
        // Allows us to configure the database (with type of the database itself)
        // Base allows to pass options to the base constructor
        public ProductDatabase(DbContextOptions<ProductDatabase> options) :base(options) { 
        }

        public DbSet<Product> Products => Set<Product>();
    }
}
