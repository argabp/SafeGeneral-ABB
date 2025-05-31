using ABB.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ABB.Infrastructure.Data.Mapping
{
    public class ApprovalMap : IEntityTypeConfiguration<Approval>
    {
        public void Configure(EntityTypeBuilder<Approval> builder)
        {
            builder.ToTable("MS_Approval", "dbo");
            builder.HasKey(k => new { k.kd_cb, k.kd_cob, k.kd_scob });
        }
    }
}