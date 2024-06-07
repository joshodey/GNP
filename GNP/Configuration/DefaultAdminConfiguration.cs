using GNP.Models;
using GNP.Utilities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GNP.Configuration
{
    public class DefaultAdminUserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            PasswordHasher<User> passwordHasher = new PasswordHasher<User>();
            User user = new User
            {
                Id = 1,
                FirstName = GNPConst.DefaultAdminRoleName,
                LastName = GNPConst.DefaultAdminRoleName,
                Email = GNPConst.DefaultAdminEmail,
                UserName = GNPConst.DefaultAdminRoleName,
                NormalizedEmail = GNPConst.DefaultAdminEmail.ToUpper(),
                NormalizedUserName = GNPConst.DefaultAdminRoleName.ToUpper(),
                UserType = UserType.Admin,
            };
            user.PasswordHash = passwordHasher.HashPassword(user, GNPConst.DefaultAdminPassword);

            builder.HasData(user);
        }
    }

    public class DefaultAdminConfiguration : IEntityTypeConfiguration<Admin>
    {
        public void Configure(EntityTypeBuilder<Admin> builder)
        {
            builder.HasData(
                new Admin()
                {
                    Id = 1,
                    UserId = 1,
                    AdminType = AdminType.Admin1,
                    CreatedAt = DateTime.UtcNow,
                });
        }
    }

    public class DefaultAdminUserRole : IEntityTypeConfiguration<IdentityUserRole<long>>
    {

        public void Configure(EntityTypeBuilder<Microsoft.AspNetCore.Identity.IdentityUserRole<long>> builder)
        {
            builder.HasData(
                new IdentityUserRole<long>()
                {
                    RoleId = 1,
                    UserId = 1,
                });
        }
    }
}
