using ABB.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ABB.Infrastructure.Data.Mapping
{
    public class ModuleNavigationMap : IEntityTypeConfiguration<ModuleNavigation>
    {
        public void Configure(EntityTypeBuilder<ModuleNavigation> builder)
        {
            builder.ToTable("TR_ModuleNavigation", "dbo");
            builder.HasKey(k => new { k.ModuleId, k.NavigationId });
        }
    }
}