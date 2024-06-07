using GNP.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GNP.Configuration
{
    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.HasData(
                new Role()
                {
                    Id = 1,
                    Name = "admin",
                    NormalizedName = "ADMIN",
                });

            builder.HasData(
                new Role()
                {
                    Id = 2,
                    Name = "applicant",
                    NormalizedName = "APPLICANT"
                });
        }
    }
}
