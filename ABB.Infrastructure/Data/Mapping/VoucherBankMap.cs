using ABB.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ABB.Infrastructure.Data.Mapping
{
    public class VoucherBankMap : IEntityTypeConfiguration<VoucherBank>
    {
        public void Configure(EntityTypeBuilder<VoucherBank> builder)
        {
            builder.ToTable("abb_voucher_bank");
            // Menentukan nama tabel di database
            // 1. UBAH PRIMARY KEY
            builder.HasKey(t => t.Id);
            
            // 2. MAPPING KOLOM ID
            builder.Property(t => t.Id)
                .HasColumnName("id")
                .ValueGeneratedOnAdd();

            // 3. UPDATE NO VOUCHER (Hapus IsRequired jika mau support sementara nanti)
            builder.Property(t => t.NoVoucher)
                .HasColumnName("no_voucher")
                .HasMaxLength(50);

            // --- Konfigurasi untuk setiap kolom ---

            builder.Property(t => t.KodeCabang)
                .HasColumnName("kode_cabang")
                .HasMaxLength(4)
                .IsRequired(); // Wajib diisi

            builder.Property(t => t.JenisVoucher)
                .HasColumnName("jenis_voucher")
                .HasMaxLength(4)
                .IsRequired(); // Wajib diisi

            builder.Property(t => t.DebetKredit)
                .HasColumnName("debet_kredit")
                .HasMaxLength(6);

           
            builder.Property(t => t.KodeAkun)
                .HasColumnName("kode_akun")
                .HasMaxLength(10);

            builder.Property(t => t.DiterimaDari)
                .HasColumnName("diterima_dari")
                .HasMaxLength(100);

            builder.Property(t => t.TanggalVoucher)
                .HasColumnName("tanggal_voucher")
                .HasColumnType("date");

            builder.Property(t => t.KodeMataUang)
                .HasColumnName("kode_matauang")
                .HasColumnType("char(3)");

            builder.Property(t => t.TotalVoucher)
                .HasColumnName("total_voucher")
                .HasColumnType("numeric(18, 2)"); // Mengatur presisi dan skala untuk desimal

            builder.Property(t => t.TotalDalamRupiah)
                .HasColumnName("total_dalam_rupiah")
                .HasColumnType("numeric(18, 2)");

            builder.Property(t => t.KeteranganVoucher)
                .HasColumnName("keterangan_voucher")
                .HasMaxLength(100);

            builder.Property(t => t.FlagPosting)
                .HasColumnName("flag_posting").HasMaxLength(1);

            builder.Property(t => t.FlagFinal)
                .HasColumnName("flag_final").HasMaxLength(1);
              

            builder.Property(t => t.TanggalInput)
                .HasColumnName("tanggal_input")
                .HasColumnType("date");

            builder.Property(t => t.TanggalUpdate)
                .HasColumnName("tanggal_update")
                .HasColumnType("date");
            builder.Property(t => t.JenisPembayaran)
            .HasColumnName("jenis_pembayaran")
            .HasMaxLength(100);

            builder.Property(t => t.KodeUserInput)
                .HasColumnName("kode_user_input")
                .HasMaxLength(25);

            builder.Property(t => t.KodeUserUpdate)
                .HasColumnName("kode_user_update")
                .HasMaxLength(25);
                
            builder.Property(t => t.KodeBank)
                .HasColumnName("kode_bank")
                .HasMaxLength(5);

            builder.Property(t => t.NoBank)
                .HasColumnName("no_bank")
                .HasMaxLength(255);

            builder.Property(t => t.TanggalPosting)
            .HasColumnName("tgl_posting")
            .HasColumnType("date");

             builder.Property(t => t.FlagSementara)
                .HasColumnName("flag_sementara"); // sesuaikan tipe data di db (bit/bool)

            builder.Property(t => t.NoVoucherSementara)
                .HasColumnName("no_voucher_smt")
                .HasMaxLength(50);
        }
    }
}