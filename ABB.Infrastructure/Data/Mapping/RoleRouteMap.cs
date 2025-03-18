using ABB.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ABB.Infrastructure.Data.Mapping
{
    public class RoleRouteMap : IEntityTypeConfiguration<RoleRoute>
    {
        public void Configure(EntityTypeBuilder<RoleRoute> builder)
        {
            builder.ToTable("TR_RoleRoute", "dbo");
            builder.HasKey(k => new { k.RoleId, k.RouteId });
        }
    }
}