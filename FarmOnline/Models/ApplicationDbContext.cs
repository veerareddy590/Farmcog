using Microsoft.EntityFrameworkCore;

namespace FarmOnline.Models
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { 

        }
        
        public DbSet<User> user { get; set; }

        public DbSet<Category> category { get; set; }

        public DbSet<Product> product { get; set; }
        public DbSet<Cart> carts { get; set; }

        public DbSet<ProductFarmer> productFarmers { get; set; }

        public DbSet<Address> address { get; set; }

        public DbSet<OrderHeader> orderHeader { get; set; }

        public DbSet<OrderDetail> orderDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>().HasData(
                new Category { CategoryId = 1,CategoryName="Vegetables"},
                new Category { CategoryId = 2,CategoryName="Fruits"},
                new Category { CategoryId = 3,CategoryName="Cereals"}
                
                );
        }





    }
}
