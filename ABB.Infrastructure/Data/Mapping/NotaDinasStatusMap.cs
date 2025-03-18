using ABB.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ABB.Infrastructure.Data.Mapping
{
    public class NotaDinasStatusMap : IEntityTypeConfiguration<NotaDinasStatus>
    {
        public void Configure(EntityTypeBuilder<NotaDinasStatus> builder)
        {
            builder.ToTable("TR_NotaDinasStatus", "dbo");
            builder.HasKey(k => new { k.id_nds, k.no_urut});
        }
    }
}