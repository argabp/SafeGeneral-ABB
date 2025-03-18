using ABB.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ABB.Infrastructure.Data.Mapping
{
    public class NavigationMap : IEntityTypeConfiguration<Navigation>
    {
        public void Configure(EntityTypeBuilder<Navigation> builder)
        {
            builder.ToTable("MS_Navigation", "dbo");
            builder.HasKey(k => k.NavigationId);
        }
    }
}