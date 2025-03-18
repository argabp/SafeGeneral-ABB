using ABB.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ABB.Infrastructure.Data.Mapping
{
    public class ViewSourceDataMap : IEntityTypeConfiguration<ViewSourceData>
    {
        public void Configure(EntityTypeBuilder<ViewSourceData> builder)
        {
            builder.ToTable("v_MS_SourceData", "dbo");
            builder.HasKey(k => k.Id);
            
            builder.Property(p => p.Premi).HasPrecision(21, 6);
            builder.Property(p => p.Disc).HasPrecision(21, 6);
            builder.Property(p => p.Komisi).HasPrecision(21, 6);
            builder.Property(p => p.BiaAkusisi).HasPrecision(21, 6);
            builder.Property(p => p.Netto).HasPrecision(21, 6);
        }
    }
}