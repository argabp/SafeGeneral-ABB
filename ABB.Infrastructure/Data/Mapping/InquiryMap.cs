using ABB.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ABB.Infrastructure.Data.Mapping
{
    public class InquiryMap : IEntityTypeConfiguration<Inquiry>
    {
        public void Configure(EntityTypeBuilder<Inquiry> builder)
        {
            builder.ToTable("uw01e", "dbo");
            builder.HasKey(k => new { k.kd_cb, k.kd_cob, k.kd_scob, k.kd_thn, k.no_pol, k.no_updt });

            builder.Property(p => p.thn_uw).HasPrecision(4, 0);
            builder.Property(p => p.pst_share_bgu).HasPrecision(9, 6);
            builder.Property(p => p.faktor_prd).HasPrecision(11, 6);
        }
    }
}