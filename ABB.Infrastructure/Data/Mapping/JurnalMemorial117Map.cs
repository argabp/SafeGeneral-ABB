using ABB.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ABB.Infrastructure.Data.Mapping
{
public class JurnalMemorial117Map : IEntityTypeConfiguration<JurnalMemorial117>
    {
        public void Configure(EntityTypeBuilder<JurnalMemorial117> builder)
        {
            builder.ToTable("abb_jurnalmemorial117");
            builder.HasKey(e => new { e.KodeCabang, e.NoVoucher }); // Composite Key

            builder.Property(t => t.KodeCabang).HasColumnName("kode_cabang").HasMaxLength(5); // Contoh
            builder.Property(t => t.NoVoucher).HasColumnName("no_voucher").HasMaxLength(50); // Contoh
            builder.Property(t => t.Keterangan).HasColumnName("keterangan").HasMaxLength(255); // Contoh
            builder.Property(t => t.KodeUserInput).HasColumnName("kode_user_input").HasMaxLength(25); // Contoh
            builder.Property(t => t.KodeUserUpdate).HasColumnName("kode_user_update").HasMaxLength(25); // Contoh
            builder.Property(t => t.FlagPosting).HasColumnName("flag_gl");
         
            // Kolom numerik dan tanggal tidak perlu MaxLength
            builder.Property(t => t.Tanggal).HasColumnName("tanggal");
            builder.Property(t => t.TanggalInput).HasColumnName("tanggal_input");
            builder.Property(t => t.TanggalUpdate).HasColumnName("tanggal_update");

        }
    }
}