using ABB.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ABB.Infrastructure.Data.Mapping
{
    public class DetailZonaMap : IEntityTypeConfiguration<DetailZona>
    {
        public void Configure(EntityTypeBuilder<DetailZona> builder)
        {
            builder.ToTable("rf12d", "dbo");
            builder.HasKey(k => new { k.kd_zona, k.kd_kls_konstr });
        }
    }
}