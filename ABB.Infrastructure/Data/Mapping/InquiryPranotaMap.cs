using ABB.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ABB.Infrastructure.Data.Mapping
{
    public class InquiryPranotaMap : IEntityTypeConfiguration<InquiryPranota>
    {
        public void Configure(EntityTypeBuilder<InquiryPranota> builder)
        {
            builder.ToTable("uw02e", "dbo");
            builder.HasKey(k => new { k.kd_cb, k.kd_cob, k.kd_scob, k.kd_thn, k.no_pol, k.no_updt, k.kd_mtu });

            builder.Property(p => p.nilai_prm).HasPrecision(21, 6);
            builder.Property(p => p.pst_dis).HasPrecision(9, 6);
            builder.Property(p => p.nilai_dis).HasPrecision(21, 6);
            builder.Property(p => p.pst_dis_fea).HasPrecision(9, 6);
            builder.Property(p => p.nilai_dis_fea).HasPrecision(21, 6);
            builder.Property(p => p.pst_dis_fleet).HasPrecision(9, 6);
            builder.Property(p => p.nilai_dis_fleet).HasPrecision(21, 6);
            builder.Property(p => p.nilai_insentif).HasPrecision(21, 6);
            builder.Property(p => p.nilai_bia_pol).HasPrecision(21, 6);
            builder.Property(p => p.nilai_bia_mat).HasPrecision(21, 6);
            builder.Property(p => p.pst_kms).HasPrecision(9, 6);
            builder.Property(p => p.nilai_kms).HasPrecision(21, 6);
            builder.Property(p => p.pst_hf).HasPrecision(9, 6);
            builder.Property(p => p.nilai_hf).HasPrecision(21, 6);
            builder.Property(p => p.pst_kms_reas).HasPrecision(9, 6);
            builder.Property(p => p.nilai_kms_reas).HasPrecision(21, 6);
            builder.Property(p => p.nilai_bia_supl).HasPrecision(21, 6);
            builder.Property(p => p.nilai_bia_pu).HasPrecision(21, 6);
            builder.Property(p => p.nilai_bia_pbtl).HasPrecision(21, 6);
            builder.Property(p => p.nilai_bia_form).HasPrecision(21, 6);
            builder.Property(p => p.nilai_kl).HasPrecision(21, 6);
            builder.Property(p => p.pst_pjk).HasPrecision(9, 6);
            builder.Property(p => p.nilai_pjk).HasPrecision(21, 6);
            builder.Property(p => p.nilai_ttl_kms).HasPrecision(21, 6);
            builder.Property(p => p.nilai_ttl_bia).HasPrecision(21, 6);
            builder.Property(p => p.nilai_ttl_ptg).HasPrecision(21, 6);
        }
    }
}