using ABB.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ABB.Infrastructure.Data.Mapping
{
    public class ApprovalDetailMap : IEntityTypeConfiguration
        <ApprovalDetail>
    {
        public void Configure(EntityTypeBuilder<ApprovalDetail> builder)
        {
            builder.ToTable("MS_ApprovalDetil", "dbo");
            builder.HasKey(k => new { k.kd_cb, k.kd_cob, k.kd_scob, k.kd_status, k.nilai_limit_awal, k.nilai_limit_akhir });
        }
    }
}