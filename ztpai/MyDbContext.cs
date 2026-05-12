using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;
using ztpai.Models;

namespace ztpai
{
    public class MyDbContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Order> Orders {  get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options) 
        { 
        }
    }

}
