using ABB.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ABB.Infrastructure.Data.Mapping
{
    public class EntriPenyelesaianPiutangMap : IEntityTypeConfiguration<EntriPenyelesaianPiutang>
    {
        public void Configure(EntityTypeBuilder<EntriPenyelesaianPiutang> builder)
        {
            builder.ToTable("abb_penyelesaian_utang");

            // --- INI BAGIAN PENTING UNTUK COMPOSITE KEY ---
            builder.HasKey(pb => new { pb.NoBukti, pb.No });

            // --- Konfigurasi Kolom ---
            builder.Property(t => t.NoBukti).HasColumnName("nomor_bukti").HasMaxLength(50).IsRequired();
            builder.Property(t => t.No).HasColumnName("no").IsRequired();
            builder.Property(t => t.KodeAkun).HasColumnName("kode_akun").HasMaxLength(10);
            builder.Property(t => t.FlagPembayaran).HasColumnName("flag_pembayaran").HasMaxLength(10);
          
            builder.Property(t => t.NoNota).HasColumnName("no_nota").HasMaxLength(100);
            builder.Property(t => t.KodeMataUang).HasColumnName("kode_mata_uang").HasMaxLength(5);
            builder.Property(t => t.TotalBayarOrg).HasColumnName("total_bayar_org");
            builder.Property(t => t.TotalBayarRp).HasColumnName("total_bayar_rp");
            builder.Property(t => t.DebetKredit).HasColumnName("debet_kredit");
            builder.Property(t => t.UserBayar).HasColumnName("user_bayar").HasMaxLength(25);
        }
    }
}