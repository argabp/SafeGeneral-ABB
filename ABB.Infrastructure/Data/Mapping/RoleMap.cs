using ABB.Domain.IdentityModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ABB.Infrastructure.Data.Mapping
{
    public class RoleMap : IEntityTypeConfiguration<AppRole>
    {
        public void Configure(EntityTypeBuilder<AppRole> builder)
        {
            builder.ToTable("MS_Role", "dbo");

            builder.Property(r => r.Id).HasColumnName("RoleId");
            builder.Property(r => r.Name).HasColumnName("RoleName");
            builder.Metadata.RemoveIndex(new[] { builder.Property(r => r.NormalizedName).Metadata });
        }
    }
}