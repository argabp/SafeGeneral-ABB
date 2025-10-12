using ABB.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ABB.Infrastructure.Data.Mapping
{
    public class InquiryObyekCISMap : IEntityTypeConfiguration<InquiryObyekCIS>
    {
        public void Configure(EntityTypeBuilder<InquiryObyekCIS> builder)
        {
            builder.ToTable("uw06e04", "dbo");
            builder.HasKey(k => new { k.kd_cb, k.kd_cob, k.kd_scob, k.kd_thn, k.no_pol, k.no_updt, k.no_rsk, k.kd_endt, k.no_oby });
        }
    }
}