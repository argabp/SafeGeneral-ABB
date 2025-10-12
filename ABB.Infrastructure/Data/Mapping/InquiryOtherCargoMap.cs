using ABB.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ABB.Infrastructure.Data.Mapping
{
    public class InquiryOtherCargoMap : IEntityTypeConfiguration<InquiryOtherCargo>
    {
        public void Configure(EntityTypeBuilder<InquiryOtherCargo> builder)
        {
            builder.ToTable("uw04e03", "dbo");
            builder.HasKey(k => new { k.kd_cb, k.kd_cob, k.kd_scob, k.kd_thn, k.no_pol, k.no_updt, k.no_rsk, k.kd_endt });
        }
    }
}