using ABB.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ABB.Infrastructure.Data.Mapping
{
    public class EntriPeriodeMap : IEntityTypeConfiguration<EntriPeriode>
    {
        public void Configure(EntityTypeBuilder<EntriPeriode> builder)
        {
            // Nama tabel di database
            builder.ToTable("ac03");

            // Tentukan Primary Key (Composite: Tahun + Bulan)
            builder.HasKey(t => new { t.ThnPrd, t.BlnPrd });

            // Mapping Kolom
            builder.Property(t => t.ThnPrd)
                .HasColumnName("thn_prd")
                .HasColumnType("decimal(5, 0)") // Sesuai request length 5
                .IsRequired();

            builder.Property(t => t.BlnPrd)
                .HasColumnName("bln_prd")
                .HasColumnType("smallint")
                .IsRequired();

            builder.Property(t => t.TglMul)
                .HasColumnName("tgl_mul")
                .HasColumnType("datetime");

            builder.Property(t => t.TglAkh)
                .HasColumnName("tgl_akh")
                .HasColumnType("datetime");

            builder.Property(t => t.FlagClosing)
                .HasColumnName("flag_closing")
                .HasMaxLength(1) // char(1)
                .IsUnicode(false); // varchar/char (bukan nvarchar)
        }
    }
}