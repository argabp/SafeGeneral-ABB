using ABB.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ABB.Infrastructure.Data.Mapping
{
    public class EntriPembayaranKasMap : IEntityTypeConfiguration<EntriPembayaranKas>
    {
        public void Configure(EntityTypeBuilder<EntriPembayaranKas> builder)
        {
            builder.ToTable("abb_pembayaran_kas");

            // --- INI BAGIAN PENTING UNTUK COMPOSITE KEY ---
            builder.HasKey(pb => new { pb.NoVoucher, pb.No });

            // --- Konfigurasi Kolom ---
            builder.Property(t => t.NoVoucher).HasColumnName("no_voucher").HasMaxLength(50).IsRequired();
            builder.Property(t => t.FlagPosting).HasColumnName("flag_posting").HasColumnType("char(1)");
            builder.Property(t => t.No).HasColumnName("no").IsRequired();
            builder.Property(t => t.KodeAkun).HasColumnName("kode_akun").HasMaxLength(10);
            builder.Property(t => t.FlagPembayaran).HasColumnName("flag_pembayaran").HasMaxLength(10);
            builder.Property(t => t.NoNota4).HasColumnName("no_nota").HasMaxLength(100);
            builder.Property(t => t.KodeMataUang).HasColumnName("kode_mata_uang").HasMaxLength(5);
            builder.Property(t => t.TotalBayar).HasColumnName("total_bayar").HasPrecision(18, 2); 
            builder.Property(t => t.UserBayar).HasColumnName("user_bayar").HasMaxLength(25);
            builder.Property(t => t.DebetKredit).HasColumnName("debet_kredit").HasMaxLength(10);
             builder.Property(t => t.Kurs).HasColumnName("kurs").HasPrecision(18, 2); 
            builder.Property(t => t.TotalDlmRupiah).HasColumnName("total_dalam_rupiah").HasPrecision(18,2);

             // tambahan sebagai audit
             builder.Property(t => t.TanggalInput)
                .HasColumnName("tgl_user_input")
                .HasColumnType("date");

            builder.Property(t => t.TanggalUpdate)
                .HasColumnName("tgl_user_update")
                .HasColumnType("date");
            
            builder.Property(t => t.KodeUserInput)
                .HasColumnName("kode_user_input")
                .HasMaxLength(255);

            builder.Property(t => t.KodeUserUpdate)
                .HasColumnName("kode_user_update")
                .HasMaxLength(255);
        }
    }
}