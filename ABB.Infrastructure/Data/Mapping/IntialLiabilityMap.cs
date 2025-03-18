using ABB.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ABB.Infrastructure.Data.Mapping
{
    public class IntialLiabilityMap : IEntityTypeConfiguration<IntialLiability>
    {
        public void Configure(EntityTypeBuilder<IntialLiability> builder)
        {
            builder.ToTable("TR_IntialLiability", "dbo");
            builder.HasKey(k => new { k.Id, k.PeriodeProses });
            
            builder.Property(p => p.BELclaim).HasPrecision(21, 6);
            builder.Property(p => p.BELexpense).HasPrecision(21, 6);
            builder.Property(p => p.RA).HasPrecision(21, 6);
            builder.Property(p => p.CSM).HasPrecision(21, 6);
            builder.Property(p => p.LRC).HasPrecision(21, 6);
        }
    }
}