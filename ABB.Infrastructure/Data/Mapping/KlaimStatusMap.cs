using ABB.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ABB.Infrastructure.Data.Mapping
{
    public class KlaimStatusMap : IEntityTypeConfiguration<KlaimStatus>
    {
        public void Configure(EntityTypeBuilder<KlaimStatus> builder)
        {
            builder.ToTable("TR_KlaimStatus", "dbo");
            builder.HasKey(k => new { k.kd_cb, k.kd_cob, k.kd_scob, k.kd_thn, k.no_kl, k.no_mts, k.no_urut });
        }
    }
}