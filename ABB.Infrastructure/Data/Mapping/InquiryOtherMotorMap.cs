using ABB.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ABB.Infrastructure.Data.Mapping
{
    public class InquiryOtherMotorMap : IEntityTypeConfiguration<InquiryOtherMotor>
    {
        public void Configure(EntityTypeBuilder<InquiryOtherMotor> builder)
        {
            builder.ToTable("uw04e02", "dbo");
            builder.HasKey(k => new { k.kd_cb, k.kd_cob, k.kd_scob, k.kd_thn, k.no_pol, k.no_updt, k.no_rsk, k.kd_endt });
        }
    }
}