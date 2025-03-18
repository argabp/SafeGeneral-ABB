using ABB.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ABB.Infrastructure.Data.Mapping
{
    public class SubsequentPAAMap : IEntityTypeConfiguration<SubsequentPAA>
    {
        public void Configure(EntityTypeBuilder<SubsequentPAA> builder)
        {
            builder.ToTable("TR_SubsequentPAA", "dbo");
            builder.HasKey(k => new { k.Id, k.PeriodeProses });
            
            builder.Property(p => p.LRCRelease).HasPrecision(21, 6);
            builder.Property(p => p.LRCReleaseIDR).HasPrecision(21, 6);
            builder.Property(p => p.LRCReleaseMovement).HasPrecision(21, 6);
            builder.Property(p => p.LRCReleaseMovementIDR).HasPrecision(21, 6);
        }
    }
}