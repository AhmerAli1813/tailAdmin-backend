using App.DataAccessLayer.EntityModel.SQL.Model;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

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
        public DbSet<OpenApiReqLog> OpenApiReqLogs { get; set; }
        public DbSet<OpenEmailLog> OpenEmailLogs { get; set; }
        public DbSet<AppsLog> AppsLogs { get; set; }

        public DbSet<Region> Regions { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<ApplicationUser>()
       .HasOne(u => u.RegionHead)
       .WithMany()
       .HasForeignKey(u => u.RegionHeadId)
       .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ApplicationUser>()
                .HasOne(u => u.LineManager)
                .WithMany()
                .HasForeignKey(u => u.LineManagerId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
