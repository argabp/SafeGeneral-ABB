using ABB.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ABB.Infrastructure.Data.Mapping
{
    public class InquiryObyekMap : IEntityTypeConfiguration<InquiryObyek>
    {
        public void Configure(EntityTypeBuilder<InquiryObyek> builder)
        {
            builder.ToTable("uw06e01", "dbo");
            builder.HasKey(k => new { k.kd_cb, k.kd_cob, k.kd_scob, k.kd_thn, k.no_pol, k.no_updt, k.no_rsk, k.kd_endt, k.no_oby });

            builder.Property(p => p.nilai_ttl_ptg).HasPrecision(21, 6);
            builder.Property(p => p.pst_adj).HasPrecision(9, 6);
        }
    }
}