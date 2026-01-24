using ABB.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ABB.Infrastructure.Data.Mapping
{
    public class RekapJurnalMap : IEntityTypeConfiguration<RekapJurnal>
    {
        public void Configure(EntityTypeBuilder<RekapJurnal> builder)
        {
            // Mapping ke nama tabel di database
            builder.ToTable("abb_rekapjurnal62");

            // PENTING: Karena tabel ini biasanya tidak punya Primary Key tunggal (ID),
            // kita set HasNoKey() agar EF Core bisa membacanya tanpa error.
            builder.HasNoKey();

            // Mapping Kolom
            builder.Property(t => t.gl_akun)
                .HasColumnName("gl_akun")
                .HasMaxLength(50); // Sesuaikan panjang karakter di DB

            builder.Property(t => t.gl_dk)
                .HasColumnName("gl_dk")
                .HasMaxLength(5); // Biasanya 'D' atau 'K'

           builder.Property(t => t.gl_nilai_idr)
                .HasColumnName("gl_nilai_idr")
                .HasColumnType("float");

            builder.Property(t => t.thn)
                .HasColumnName("thn");

            builder.Property(t => t.bln)
                .HasColumnName("bln");
        }
    }
}