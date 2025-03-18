using ABB.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ABB.Infrastructure.Data.Mapping
{
    public class DetailPertanggunganKendaraanMap : IEntityTypeConfiguration<DetailPertanggunganKendaraan>
    {
        public void Configure(EntityTypeBuilder<DetailPertanggunganKendaraan> builder)
        {
            builder.ToTable("dp01d", "dbo");
            builder.HasKey(k => new { k.kd_cob, k.kd_scob, k.kd_jns_ptg, k.no_urut });

            builder.Property(p => p.nilai_tsi_tjh_mul).HasPrecision(21, 6);
            builder.Property(p => p.nilai_tsi_tjh_akh).HasPrecision(21, 6);
            builder.Property(p => p.pst_rate_tjh).HasPrecision(9, 6);
            builder.Property(p => p.nilai_prm_tjh).HasPrecision(21, 6);
            builder.Property(p => p.nilai_tsi_tjp).HasPrecision(21, 6);
            builder.Property(p => p.nilai_prm_tjp).HasPrecision(21, 6);
            builder.Property(p => p.pst_rate_pad).HasPrecision(9, 6);
            builder.Property(p => p.pst_rate_pap).HasPrecision(9, 6);
        }
    }
}