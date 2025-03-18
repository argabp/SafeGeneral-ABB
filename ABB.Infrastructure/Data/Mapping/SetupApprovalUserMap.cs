using ABB.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ABB.Infrastructure.Data.Mapping
{
    public class SetupApprovalUserMap : IEntityTypeConfiguration<SetupApprovalUser>
    {
        public void Configure(EntityTypeBuilder<SetupApprovalUser> builder)
        {
            builder.ToTable("MS_SetupApprovalUser", "dbo");
            builder.HasKey(k => new { k.kd_cb, k.kd_rk, k.kd_approval, k.kd_proses, k.userid });
        }
    }
}