using ABB.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ABB.Infrastructure.Data.Mapping
{
    public class RoleLevelMap : IEntityTypeConfiguration<RoleLevel>
    {
        public void Configure(EntityTypeBuilder<RoleLevel> builder)
        {
            builder.ToTable("TR_RoleLevel", "dbo");
            builder.HasKey(k => k.RoleLevelId);
        }
    }
}