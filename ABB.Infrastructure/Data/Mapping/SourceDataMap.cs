using ABB.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ABB.Infrastructure.Data.Mapping
{
    public class SourceDataMap : IEntityTypeConfiguration<SourceData>
    {
        public void Configure(EntityTypeBuilder<SourceData> builder)
        {
            builder.ToTable("MS_SourceData", "dbo");
            builder.HasKey(k => k.Id);
            
            builder.Property(p => p.Premi).HasPrecision(21, 6);
            builder.Property(p => p.Disc).HasPrecision(21, 6);
            builder.Property(p => p.Komisi).HasPrecision(21, 6);
            builder.Property(p => p.BiaPolis).HasPrecision(21, 6);
            builder.Property(p => p.BiaMaterai).HasPrecision(21, 6);
            builder.Property(p => p.BiaAkusisi).HasPrecision(21, 6);
            builder.Property(p => p.Netto).HasPrecision(21, 6);
            builder.Property(p => p.Klaim).HasPrecision(21, 6);
            builder.Property(p => p.Ppn).HasPrecision(21, 6);
            builder.Property(p => p.Pph).HasPrecision(21, 6);
            builder.Property(p => p.Release).HasPrecision(21, 6);
        }
    }
}