using ABB.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ABB.Infrastructure.Data.Mapping
{
    public class TemplateLapKeuMap : IEntityTypeConfiguration<TemplateLapKeu>
    {
        public void Configure(EntityTypeBuilder<TemplateLapKeu> builder)
        {
            builder.ToTable("abb_template_lapkeu");
            builder.HasKey(t => t.Id);

            // UPDATE BAGIAN INI
            builder.Property(t => t.Id)
                .HasColumnName("id");
               

            builder.Property(t => t.TipeLaporan).HasColumnName("tipe_laporan").HasMaxLength(255);
            builder.Property(t => t.TipeBaris).HasColumnName("tipe_baris").HasMaxLength(255);
            builder.Property(t => t.Deskripsi).HasColumnName("deskripsi").HasMaxLength(255);
            builder.Property(t => t.Rumus).HasColumnName("rumus").HasMaxLength(255);
            builder.Property(t => t.Level).HasColumnName("level").HasMaxLength(255);
            builder.Property(t => t.Urutan).HasColumnName("urutan");
        }
    }
}