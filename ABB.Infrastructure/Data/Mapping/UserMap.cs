using ABB.Domain.IdentityModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ABB.Infrastructure.Data.Mapping
{
    public class UserMap : IEntityTypeConfiguration<AppUser>
    {
        public void Configure(EntityTypeBuilder<AppUser> builder)
        {
            builder.ToTable("MS_User", "dbo");

            builder.Property(r => r.Id).HasColumnName("UserId");
            builder.Property(r => r.UserName).HasColumnName("Username");
            builder.Property(r => r.PasswordHash).HasColumnName("Password");
            builder.Property(r => r.PasswordExpiredDate);
        }
    }
}