using ABB.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ABB.Infrastructure.Data.Mapping
{
    public class PesertaSiramaMap : IEntityTypeConfiguration<PesertaSirama>
    {
        public void Configure(EntityTypeBuilder<PesertaSirama> builder)
        {
            builder.ToTable("TR_PesertaSirama", "dbo");
            builder.HasKey(k => new { k.kd_cb, k.kd_product, k.kd_thn, k.kd_rk, k.no_sppa, k.no_updt });

            builder.Property(p => p.loading_prm).HasPrecision(21, 6);
            builder.Property(p => p.nilai_prm).HasPrecision(21, 6);
            builder.Property(p => p.nilai_ptg).HasPrecision(21, 6);
            builder.Property(p => p.total_prm).HasPrecision(21, 6);
            builder.Property(p => p.pst_rate).HasPrecision(9, 6);
        }
    }
}