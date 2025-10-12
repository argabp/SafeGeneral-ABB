using ABB.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ABB.Infrastructure.Data.Mapping
{
    public class InquiryPranotaKoasMap : IEntityTypeConfiguration<InquiryPranotaKoas>
    {
        public void Configure(EntityTypeBuilder<InquiryPranotaKoas> builder)
        {
            builder.ToTable("uw03e", "dbo");
            builder.HasKey(k => new { k.kd_cb, k.kd_cob, k.kd_scob, k.kd_thn, 
                k.no_pol, k.no_updt, k.kd_mtu, k.kd_grp_pas, k.kd_rk_pas });

            builder.Property(p => p.pst_share).HasPrecision(9, 6);
            builder.Property(p => p.nilai_prm).HasPrecision(21, 6);
            builder.Property(p => p.pst_dis).HasPrecision(9, 6);
            builder.Property(p => p.nilai_dis).HasPrecision(21, 6);
            builder.Property(p => p.pst_kms).HasPrecision(9, 6);
            builder.Property(p => p.nilai_kms).HasPrecision(21, 6);
            builder.Property(p => p.pst_hf).HasPrecision(9, 6);
            builder.Property(p => p.nilai_hf).HasPrecision(21, 6);
            builder.Property(p => p.nilai_kl).HasPrecision(21, 6);
            builder.Property(p => p.pst_pjk).HasPrecision(9, 6);
            builder.Property(p => p.nilai_pjk).HasPrecision(21, 6);
        }
    }
}