using ABB.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ABB.Infrastructure.Data.Mapping
{
    public class HeaderPenyelesaianUtangMap : IEntityTypeConfiguration<HeaderPenyelesaianUtang>
    {
        public void Configure(EntityTypeBuilder<HeaderPenyelesaianUtang> builder)
        {
            builder.ToTable("abb_header_penyelesaian_utang");
            builder.HasKey(e => new { e.KodeCabang, e.NomorBukti }); // Composite Key

            builder.Property(t => t.KodeCabang).HasColumnName("kode_cabang").HasMaxLength(3); // Contoh
            builder.Property(t => t.NomorBukti).HasColumnName("nomor_bukti").HasMaxLength(50); // Contoh
            builder.Property(t => t.JenisPenyelesaian).HasColumnName("jenis_penyelesaian").HasMaxLength(25); // Contoh
            builder.Property(t => t.KodeVoucherAcc).HasColumnName("kode_voucher_acc").HasMaxLength(50); // Contoh
            builder.Property(t => t.MataUang).HasColumnName("mata_uang").HasMaxLength(3); // Contoh
            builder.Property(t => t.DebetKredit).HasColumnName("debet_kredit").HasMaxLength(10); // Contoh
            builder.Property(t => t.Keterangan).HasColumnName("keterangan").HasMaxLength(255); // Contoh
            builder.Property(t => t.KodeAkun).HasColumnName("kode_akun").HasMaxLength(10); // Contoh
            builder.Property(t => t.KodeUserInput).HasColumnName("kode_user_input").HasMaxLength(25); // Contoh
            builder.Property(t => t.KodeUserUpdate).HasColumnName("kode_user_update").HasMaxLength(25); // Contoh
            builder.Property(t => t.FlagPosting).HasColumnName("flag_posting");

            // Kolom numerik dan tanggal tidak perlu MaxLength
            builder.Property(t => t.Tanggal).HasColumnName("tanggal");
            builder.Property(t => t.TotalOrg).HasColumnName("total_org");
            builder.Property(t => t.TotalRp).HasColumnName("total_rp");
            builder.Property(t => t.TanggalInput).HasColumnName("tanggal_input");
            builder.Property(t => t.TanggalUpdate).HasColumnName("tanggal_update");

        }
    }
}