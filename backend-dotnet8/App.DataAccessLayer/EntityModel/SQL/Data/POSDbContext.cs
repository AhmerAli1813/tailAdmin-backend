using App.DataAccessLayer.EntityModel.SQL.Model;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace App.DataAccessLayer.EntityModel.SQL.Data
{
    public class POSDbContext : IdentityDbContext<ApplicationUser>
    {
        public POSDbContext(DbContextOptions<POSDbContext> options) : base(options)
        {

        }
        protected POSDbContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<OpenApiReqLog> OpenApiReqLogs { get; set; }
        public DbSet<OpenEmailLog> OpenEmailLogs { get; set; }
        public DbSet<AppsLog> AppsLogs { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Stock> Stocks { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Dispatch> Dispatches { get; set; }
        public DbSet<Investment> Investments { get; set; }
        public DbSet<PriceManagement> PriceManagements { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
      
        }
    }


}
