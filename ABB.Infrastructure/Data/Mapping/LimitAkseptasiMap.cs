using ABB.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ABB.Infrastructure.Data.Mapping
{
    public class LimitAkseptasiMap : IEntityTypeConfiguration<LimitAkseptasi>
    {
        public void Configure(EntityTypeBuilder<LimitAkseptasi> builder)
        {
            builder.ToTable("MS_LimitAkseptasi", "dbo");
            builder.HasKey(k => new { k.kd_cb, k.kd_cob, k.kd_scob });
        }
    }
}