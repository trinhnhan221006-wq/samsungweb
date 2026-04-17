using Microsoft.AspNetCore.Identity.EntityFrameworkCore;    
using Microsoft.EntityFrameworkCore;
using samsungweb.Models;
using samsungweb.Models;
using SamsungWeb.Models;

namespace samsungweb.Data
{
    // Kế thừa từ DbContext của Entity Framework
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        // Khai báo các bảng sẽ được tạo trong SQL Server
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }

        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }

        public DbSet<HomeDisplay> HomeDisplays { get; set; }
    }
}