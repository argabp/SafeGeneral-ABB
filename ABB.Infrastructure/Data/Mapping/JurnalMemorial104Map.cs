using ABB.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ABB.Infrastructure.Data.Mapping
{
    public class JurnalMemorial104Map : IEntityTypeConfiguration<JurnalMemorial104>
    {
        public void Configure(EntityTypeBuilder<JurnalMemorial104> builder)
        {
            builder.ToTable("abb_jurnalmemorial104");

             builder.HasKey(t => t.NoVoucher);

            builder.Property(t => t.NoVoucher)
                .HasColumnName("no_voucher"); 

            builder.Property(t => t.KodeCabang)
                .HasColumnName("kode_cabang");   

            builder.Property(t => t.Tanggal)
                .HasColumnName("tanggal");   

            builder.Property(t => t.Keterangan)
                .HasColumnName("keterangan"); 

            builder.Property(t => t.TanggalInput)
                .HasColumnName("tanggal_input");

            builder.Property(t => t.KodeUserInput)
                .HasColumnName("kode_user_input");

            builder.Property(t => t.TanggalUpdate)
                .HasColumnName("tanggal_update");

            builder.Property(t => t.KodeUserUpdate)
                .HasColumnName("kode_user_update");

            builder.Property(t => t.FlagGL)
                .HasColumnName("flag_gl");  
        }
    }
}