using GNP.Configuration;
using GNP.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GNP.Context
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        
        public DbSet<User> User { get; set; }
        public DbSet<Applicant> Applicants { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<ApplicationForm> ApplicationForms { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new RoleConfiguration());
            builder.ApplyConfiguration(new DefaultAdminUserConfiguration());
            builder.ApplyConfiguration(new DefaultAdminConfiguration());
            //builder.ApplyConfiguration(new DefaultAdminUserRole());

            base.OnModelCreating(builder);
        }
    }
}
