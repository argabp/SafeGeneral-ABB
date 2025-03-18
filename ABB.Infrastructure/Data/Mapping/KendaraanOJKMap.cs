using ABB.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ABB.Infrastructure.Data.Mapping
{
    public class KendaraanOJKMap : IEntityTypeConfiguration<KendaraanOJK>
    {
        public void Configure(EntityTypeBuilder<KendaraanOJK> builder)
        {
            builder.ToTable("rf15d01", "dbo");
            builder.HasKey(k => new { k.kd_kategori, k.kd_jns_ptg, k.kd_wilayah, k.no_kategori });

            builder.Property(p => p.nilai_ptg_mul).HasPrecision(21, 6);
            builder.Property(p => p.nilai_ptg_akh).HasPrecision(21, 6);
            builder.Property(p => p.pst_rate_prm_min).HasPrecision(9, 6);
            builder.Property(p => p.pst_rate_prm_max).HasPrecision(9, 6);
        }
    }
}