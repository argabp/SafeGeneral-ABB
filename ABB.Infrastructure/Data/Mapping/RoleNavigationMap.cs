using ABB.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ABB.Infrastructure.Data.Mapping
{
    public class RoleNavigationMap : IEntityTypeConfiguration<RoleNavigation>
    {
        public void Configure(EntityTypeBuilder<RoleNavigation> builder)
        {
            builder.ToTable("TR_RoleNavigation", "dbo");
            builder.HasKey(k => new { k.RoleId, k.NavigationId });
        }
    }
}