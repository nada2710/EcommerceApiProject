using EcommerceProjectDAL.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceProjectDAL.Data.DbHelper
{
    public class EcommerceProjectContext: IdentityDbContext<ApplicationUser>
    {
        public EcommerceProjectContext(DbContextOptions<EcommerceProjectContext> options) : base(options) { }
       
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            //Table Per Type(table=>AppUser,Table=>Admin,Table=>Customer)
            builder.Entity<Admin>().ToTable("Admin");
            builder.Entity<Customer>().ToTable("Customer");

            //to put date and time automatic when creat order
            builder.Entity<Order>()
                   .Property(o => o.CreatedDate)
                   .HasDefaultValueSql("GETDATE()");

            ////if i remove cart then shoppingCartItems will be removed
            //builder.Entity<ShoppingCart>()
            //       .HasMany(c => c.shoppingCartItems)
            //       .WithOne()
            //       .OnDelete(DeleteBehavior.Cascade);
            //soft delete
            builder.Entity<ApplicationUser>()
                .HasQueryFilter(a => a.IsDeleted==false);
            builder.Entity<Category>()
                .HasQueryFilter(a => a.IsDeleted==false);
            builder.Entity<Order>()
                .HasQueryFilter(a => a.IsDeleted==false);
            builder.Entity<OrderItem>()
                .HasQueryFilter(a => a.IsDeleted==false);
            builder.Entity<Payment>()
                .HasQueryFilter(a => a.IsDeleted==false);
            builder.Entity<Product>()
                .HasQueryFilter(a => a.IsDeleted==false);
            builder.Entity<Review>()
                .HasQueryFilter(a => a.IsDeleted==false);
            builder.Entity<ShoppingCart>()
                .HasQueryFilter(a => a.IsDeleted==false);
            builder.Entity<ShoppingCartItem>()
                .HasQueryFilter(a => a.IsDeleted==false);

            base.OnModelCreating(builder);
        }
        public DbSet<ApplicationUser> applicationUsers { get; set; }
        public DbSet<Admin> admins { get; set; }
        public DbSet<Customer> customers { get; set; }
        public DbSet<Category> categories { get; set; }
        public DbSet<Order> orders { get; set; }
        public DbSet<OrderItem>orderItems { get; set; }
        public DbSet<Payment>payments { get; set; }
        public DbSet<Product>products { get; set; }
        public DbSet<Review> reviews { get; set; }
        public DbSet<ShoppingCart> shoppingCarts { get; set; }
        public DbSet<ShoppingCartItem> shoppingCartItems { get; set; }
    }
}
