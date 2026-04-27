using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;
using ztpai.Models;

namespace ztpai
{
    public class MyDbContext : IdentityDbContext<User, IdentityRole<int>, int>
    {
        public DbSet<Product> Products { get; set; }
        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options) 
        { 
        }
    }

}
