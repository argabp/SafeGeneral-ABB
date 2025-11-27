using ABB.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ABB.Infrastructure.Data.Mapping
{
    public class TipeAkun104Map : IEntityTypeConfiguration<TipeAkun104>
    {
        public void Configure(EntityTypeBuilder<TipeAkun104> builder)
        {
            builder.ToTable("abb_tipe_akun_104");

            // Tentukan gl_kode sebagai Primary Key
            builder.HasKey(c => c.Kode); 
            
            builder.Property(c => c.Kode).HasColumnName("kode");
            builder.Property(c => c.NamaTipe).HasColumnName("nm_tipe");
            builder.Property(c => c.Pos).HasColumnName("pos");
            builder.Property(c => c.Kasbank).HasColumnName("kasbank");
        }
    }
}