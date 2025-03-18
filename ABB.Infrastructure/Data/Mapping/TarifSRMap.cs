using ABB.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ABB.Infrastructure.Data.Mapping
{
    public class TarifSRMap : IEntityTypeConfiguration<TarifSR>
    {
        public void Configure(EntityTypeBuilder<TarifSR> builder)
        {
            builder.ToTable("MS_TarifSR", "dbo");
            builder.HasKey(k => new { k.kd_cb, k.kd_rk, k.masa_ptg, k.usia_awal, k.usia_akhr });

            builder.Property(p => p.pst_rate).HasPrecision(9, 6);
        }
    }
}