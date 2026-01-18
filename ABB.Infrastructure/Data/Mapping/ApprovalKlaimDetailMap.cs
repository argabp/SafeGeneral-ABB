using ABB.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ABB.Infrastructure.Data.Mapping
{
    public class ApprovalKlaimDetailMap : IEntityTypeConfiguration<ApprovalKlaimDetail>
    {
        public void Configure(EntityTypeBuilder<ApprovalKlaimDetail> builder)
        {
            builder.ToTable("MS_ApprovalKlaimDetilSign", "dbo");
            builder.HasKey(k => new { k.kd_cb, k.kd_cob, k.kd_scob, k.kd_user, k.kd_status, k.kd_user_sign });
        }
    }
}