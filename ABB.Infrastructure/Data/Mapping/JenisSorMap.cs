using ABB.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ABB.Infrastructure.Data.Mapping
{
    public class JenisSorMap : IEntityTypeConfiguration<JenisSor>
    {
        public void Configure(EntityTypeBuilder<JenisSor> builder)
        {
            builder.ToTable("rf18", "dbo");
            builder.HasKey(k => k.kd_jns_sor);
        }
    }
}