using App.DataAccessLayer.EntityModel.SQL.Model;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace App.DataAccessLayer.EntityModel.SQL.Data
{
    public class JSIL_IdentityDbContext : IdentityDbContext<ApplicationUser>
    {
        public JSIL_IdentityDbContext(DbContextOptions<JSIL_IdentityDbContext> options) : base(options)
        {

        }
        protected JSIL_IdentityDbContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Log> Logs { get; set; }
        public DbSet<Message>  Messages { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
