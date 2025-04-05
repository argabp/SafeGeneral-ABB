using ABB.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ABB.Infrastructure.Data.Mapping
{
    public class CopyEndorsMap : IEntityTypeConfiguration<CopyEndors>
    {
        public void Configure(EntityTypeBuilder<CopyEndors> builder)
        {
            builder.ToTable("uw04c", "dbo");
            builder.HasKey(k => new { k.kd_cb, k.kd_cob, k.kd_scob, k.kd_thn, k.no_pol, k.no_updt, k.no_rsk, k.kd_endt });

            builder.Property(p => p.pst_rate_prm).HasPrecision(9, 6);
            builder.Property(p => p.nilai_prm).HasPrecision(21, 6);
            builder.Property(p => p.pst_dis).HasPrecision(9, 6);
            builder.Property(p => p.nilai_dis).HasPrecision(21, 6);
            builder.Property(p => p.pst_kms).HasPrecision(9, 6);
            builder.Property(p => p.nilai_kms).HasPrecision(21, 6);
            builder.Property(p => p.nilai_insentif).HasPrecision(21, 6);
            builder.Property(p => p.nilai_kl).HasPrecision(21, 6);
            builder.Property(p => p.nilai_ttl_ptg).HasPrecision(21, 6);
            builder.Property(p => p.faktor_prd).HasPrecision(11, 6);
            builder.Property(p => p.pst_share_bgu).HasPrecision(9, 6);
        }
    }
}