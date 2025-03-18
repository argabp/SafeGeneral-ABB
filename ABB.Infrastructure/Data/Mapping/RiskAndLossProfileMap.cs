using ABB.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ABB.Infrastructure.Data.Mapping
{
    public class RiskAndLossProfileMap : IEntityTypeConfiguration<RiskAndLossProfile>
    {
        public void Configure(EntityTypeBuilder<RiskAndLossProfile> builder)
        {
            builder.ToTable("dp16", "dbo");
            builder.HasKey(k => new { k.kd_cob, k.nomor });

            builder.Property(p => p.bts1).HasPrecision(21, 6);
            builder.Property(p => p.bts2).HasPrecision(21, 6);
        }
    }
}