using ABB.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ABB.Infrastructure.Data.Mapping
{
    public class EntriMappingMap : IEntityTypeConfiguration<EntriMapping>
    {
        public void Configure(EntityTypeBuilder<EntriMapping> builder)
        {
            // Nama tabel di database
            builder.ToTable("abb_mapcoa");

            // Tentukan Primary Key (Composite: Tahun + Bulan)
            builder.HasKey(t => new { t.gl_akun104, t.gl_akun117 });

            // Mapping Kolom
            builder.Property(t => t.gl_akun104)
                .HasColumnName("gl_akun104");

            builder.Property(t => t.gl_akun117)
                .HasColumnName("gl_akun117");

        }
    }
}