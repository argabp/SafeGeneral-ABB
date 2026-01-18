using ABB.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ABB.Infrastructure.Data.Mapping
{
    public class ApprovalKlaimMap : IEntityTypeConfiguration<ApprovalKlaim>
    {
        public void Configure(EntityTypeBuilder<ApprovalKlaim> builder)
        {
            builder.ToTable("MS_ApprovalKlaim", "dbo");
            builder.HasKey(k => new { k.kd_cb, k.kd_cob, k.kd_scob });
        }
    }
}