using ABB.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ABB.Infrastructure.Data.Mapping
{
    public class TipeAkun117Map : IEntityTypeConfiguration<TipeAkun117>
    {
        public void Configure(EntityTypeBuilder<TipeAkun117> builder)
        {
            builder.ToTable("abb_tipe_coa117");

            // Tentukan gl_kode sebagai Primary Key
            builder.HasKey(c => c.Kode); 
            
            builder.Property(c => c.Kode).HasColumnName("gl_type");
            builder.Property(c => c.NamaTipe).HasColumnName("f_nama");
            builder.Property(c => c.Pos).HasColumnName("gl_pos");
            builder.Property(c => c.DebetKredit).HasColumnName("gl_dk");
        }
    }
}