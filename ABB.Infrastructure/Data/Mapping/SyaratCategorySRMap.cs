using ABB.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ABB.Infrastructure.Data.Mapping
{
    public class SyaratCategorySRMap : IEntityTypeConfiguration<SyaratCategorySR>
    {
        public void Configure(EntityTypeBuilder<SyaratCategorySR> builder)
        {
            builder.ToTable("MS_SyaratCategorySR", "dbo");
            builder.HasKey(k => new { k.kd_cb, k.kd_rk, k.nilai_ptg_awal, k.nilai_ptg_akhr, k.usia_awal, k.usia_akhr });

            builder.Property(p => p.nilai_ptg_awal).HasPrecision(21, 6);
            builder.Property(p => p.nilai_ptg_akhr).HasPrecision(21, 6);
        }
    }
}