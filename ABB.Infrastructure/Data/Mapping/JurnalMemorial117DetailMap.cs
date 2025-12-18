using ABB.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ABB.Infrastructure.Data.Mapping
{
    public class JurnalMemorial117DetailMap : IEntityTypeConfiguration<JurnalMemorial117Detail>
    {
        public void Configure(EntityTypeBuilder<JurnalMemorial117Detail> builder)
        {
            // builder.ToTable("abb_penyelesaianutang");
            builder.ToTable("abb_jurnalmemorial117_detail");

            // --- INI BAGIAN PENTING UNTUK COMPOSITE KEY ---
            builder.HasKey(pb => new { pb.NoVoucher, pb.No });

            // --- Konfigurasi Kolom ---
            builder.Property(t => t.NoVoucher).HasColumnName("no_voucher").HasMaxLength(50).IsRequired();
            builder.Property(t => t.No).HasColumnName("no").IsRequired();
            builder.Property(t => t.KodeAkun).HasColumnName("kode_akun").HasMaxLength(10);
            builder.Property(t => t.FlagPosting).HasColumnName("flag_posting").HasMaxLength(10);
            builder.Property(t => t.NoNota).HasColumnName("no_nota").HasMaxLength(100);
            builder.Property(t => t.KodeMataUang).HasColumnName("kode_mata_uang").HasMaxLength(5);
            builder.Property(t => t.NilaiDebet).HasColumnName("nilai_debet");
            builder.Property(t => t.NilaiDebetRp).HasColumnName("nilai_debet_rp");
            builder.Property(t => t.NilaiKredit).HasColumnName("nilai_kredit");
            builder.Property(t => t.NilaiKreditRp).HasColumnName("nilai_kredit_rp");
            builder.Property(t => t.TanggalBayar).HasColumnName("tanggal_bayar");
            builder.Property(t => t.UserBayar).HasColumnName("user_bayar").HasMaxLength(25);
            builder.Property(t => t.KodeUserInput).HasColumnName("kode_user_input").HasMaxLength(25); // Contoh
            builder.Property(t => t.KodeUserUpdate).HasColumnName("kode_user_update").HasMaxLength(25); // Contoh
            builder.Property(t => t.TanggalUserInput).HasColumnName("tgl_user_input");
            builder.Property(t => t.TanggalUserUpdate).HasColumnName("tgl_user_update");
        }
    }
}