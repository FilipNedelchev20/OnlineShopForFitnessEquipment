using FitnessEquipmentShop.Data.Models.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FitnessEquipmentShop.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext()
        {
            
        }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        
            public DbSet<User> Users { get; set; }
            public DbSet<Product> Products { get; set; }
            public DbSet<Category> Categories { get; set; }
            public DbSet<CartItem> CartItems { get; set; }
            public DbSet<Order> Orders { get; set; }
            public DbSet<OrderDetail> OrderDetails { get; set; }
            public DbSet<Review> Reviews { get; set; }
            public DbSet<Wishlist> Wishlist { get; set; }
            public DbSet<Address> Address { get; set; }
       
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
           
            modelBuilder.Entity<User>()
                .HasMany(u => u.Addresses)
                .WithOne(a => a.User)
                .HasForeignKey(a => a.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>()
                .HasMany(u => u.Wishlists)
                .WithOne(w => w.User)
                .HasForeignKey(w => w.UserId)
                .OnDelete(DeleteBehavior.Cascade);

          
            modelBuilder.Entity<User>()
                .HasMany(u => u.Orders)
                .WithOne(o => o.User)
                .HasForeignKey(o => o.UserId)
                .OnDelete(DeleteBehavior.Cascade);

         
            modelBuilder.Entity<User>()
                .HasMany(u => u.Reviews)
                .WithOne(r => r.User)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            
            modelBuilder.Entity<Product>()
                .HasMany(p => p.Reviews)
                .WithOne(r => r.Product)
                .HasForeignKey(r => r.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

           
            modelBuilder.Entity<Product>()
                .HasMany(p => p.Wishlists)
                .WithOne(w => w.Product)
                .HasForeignKey(w => w.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

          
            modelBuilder.Entity<Product>()
                .HasMany(p => p.OrderDetails)
                .WithOne(od => od.Product)
                .HasForeignKey(od => od.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

           
            modelBuilder.Entity<Order>()
                .HasMany(o => o.OrderDetails)
                .WithOne(od => od.Order)
                .HasForeignKey(od => od.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

          
            modelBuilder.Entity<User>()
                .HasMany(u => u.Wishlists)
                .WithOne(c => c.User)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade);

           
            modelBuilder.Entity<Product>()
                .HasMany(p => p.OrderDetails)
                .WithOne(ci => ci.Product)
                .HasForeignKey(ci => ci.ProductId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
