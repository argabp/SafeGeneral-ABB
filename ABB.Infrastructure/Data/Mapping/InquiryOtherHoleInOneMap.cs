using ABB.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ABB.Infrastructure.Data.Mapping
{
    public class InquiryOtherHoleInOneMap : IEntityTypeConfiguration<InquiryOtherHoleInOne>
    {
        public void Configure(EntityTypeBuilder<InquiryOtherHoleInOne> builder)
        {
            builder.ToTable("uw04e08", "dbo");
            builder.HasKey(k => new { k.kd_cb, k.kd_cob, k.kd_scob, k.kd_thn, k.no_pol, k.no_updt, k.no_rsk, k.kd_endt });
        }
    }
}