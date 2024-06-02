using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GNP.Configuration
{
    public class RoleConfiguration : IEntityTypeConfiguration<IdentityRole>
    {
        public void Configure(EntityTypeBuilder<IdentityRole> builder)
        {
            builder.HasData(
                new IdentityRole()
                {
                    Id = "1",
                    Name = "admin",
                    NormalizedName = "ADMIN",
                });

            builder.HasData(
                new IdentityRole()
                {
                    Id = "2",
                    Name = "applicant",
                    NormalizedName = "APPLICANT"
                });
        }
    }
}
