using ABB.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ABB.Infrastructure.Data.Mapping
{
    public class DetailJurnalMemorial104Map : IEntityTypeConfiguration<DetailJurnalMemorial104>
    {
        public void Configure(EntityTypeBuilder<DetailJurnalMemorial104> builder)
        {
            builder.ToTable("abb_jurnalmemorial104_detail");

            builder.HasKey(t => new { t.NoVoucher, t.No });

            builder.Property(t => t.NoVoucher)
                .HasColumnName("no_voucher");   

            builder.Property(t => t.FlagPosting)
                .HasColumnName("flag_posting");   

            builder.Property(t => t.No)
                .HasColumnName("no");   

            builder.Property(t => t.KodeAkun)
                .HasColumnName("kode_akun"); 

            builder.Property(t => t.NoNota)
                .HasColumnName("no_nota");

            builder.Property(t => t.KodeMataUang)
                .HasColumnName("kode_mata_uang");

            builder.Property(t => t.NilaiDebet)
                .HasColumnName("nilai_debet");

            builder.Property(t => t.NilaiDebetRP)
                .HasColumnName("nilai_debet_rp");

            builder.Property(t => t.NilaiKredit)
                .HasColumnName("nilai_kredit");  

            builder.Property(t => t.NilaiKreditRP)
                .HasColumnName("nilai_kredit_rp");

            builder.Property(t => t.UserBayar)
                .HasColumnName("user_bayar");

            builder.Property(t => t.TanggalBayar)
                .HasColumnName("tanggal_bayar");

            builder.Property(t => t.KodeUserInput)
                .HasColumnName("kode_user_input");

            builder.Property(t => t.KodeUserUpdate)
                .HasColumnName("kode_user_update");

            builder.Property(t => t.TanggalUserUpdate)
                .HasColumnName("tgl_user_update");

            builder.Property(t => t.TanggalUserInput)
                .HasColumnName("tgl_user_input");

           
        }
    }
}