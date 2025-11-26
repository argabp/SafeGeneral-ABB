using ABB.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ABB.Infrastructure.Data.Mapping
{
    public class VoucherKasMap : IEntityTypeConfiguration<VoucherKas>
    {
        public void Configure(EntityTypeBuilder<VoucherKas> builder)
        {
            
            builder.ToTable("abb_voucher_kas");
            
            // ditambahin untuk pk
            builder.HasKey(t => t.NoVoucher);

            builder.Property(t => t.KodeCabang)
                .HasColumnName("kode_cabang")
                .HasMaxLength(4)
                .IsRequired();

            builder.Property(t => t.JenisVoucher)
                .HasColumnName("jenis_voucher")
                .HasMaxLength(4)
                .IsRequired();

            builder.Property(t => t.DebetKredit)
                .HasColumnName("debet_kredit")
                .HasMaxLength(5);

            builder.Property(t => t.NoVoucher)
                .HasColumnName("no_voucher")
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(t => t.KodeAkun)
                .HasColumnName("kode_akun")
                .HasMaxLength(10);

            builder.Property(t => t.DibayarKepada)
                .HasColumnName("dibayar_kepada")
                .HasMaxLength(100);

            builder.Property(t => t.TanggalVoucher)
                .HasColumnName("tanggal_voucher")
                .HasColumnType("date");

            builder.Property(t => t.KodeMataUang)
                .HasColumnName("kode_matauang")
                .HasMaxLength(3);

            builder.Property(t => t.TotalVoucher)
                .HasColumnName("total_voucher")
                .HasPrecision(18,2);

            builder.Property(t => t.TotalDalamRupiah)
                .HasColumnName("total_dalam_rupiah")
                .HasPrecision(18,2);

            builder.Property(t => t.KeteranganVoucher)
                .HasColumnName("keterangan_voucher")
                .HasMaxLength(100);

            builder.Property(t => t.FlagPosting)
                .HasColumnName("flag_posting")
                .HasMaxLength(1);

            builder.Property(t => t.FlagFinal)
            .HasColumnName("flag_final").HasMaxLength(1);

            builder.Property(t => t.TanggalInput)
                .HasColumnName("tanggal_input")
                .HasColumnType("date");

            builder.Property(t => t.TanggalUpdate)
                .HasColumnName("tanggal_update")
                .HasColumnType("date");

            builder.Property(t => t.KodeUserInput)
                .HasColumnName("kode_user_input")
                .HasMaxLength(25);

            builder.Property(t => t.KodeUserUpdate)
                .HasColumnName("kode_user_update")
                .HasMaxLength(25);

            builder.Property(t => t.JenisPembayaran)
            .HasColumnName("jenis_pembayaran")
            .HasMaxLength(100);

              builder.Property(t => t.KodeKas)
            .HasColumnName("kode_kas")
            .HasMaxLength(5);
        }
    }
}