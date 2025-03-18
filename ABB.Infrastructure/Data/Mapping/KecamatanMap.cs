using ABB.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ABB.Infrastructure.Data.Mapping
{
    public class KecamatanMap : IEntityTypeConfiguration<Kecamatan>
    {
        public void Configure(EntityTypeBuilder<Kecamatan> builder)
        {
            builder.ToTable("rf07_02", "dbo");
            builder.HasKey(k => new { k.kd_prop, k.kd_kab, k.kd_kec });
        }
    }
}