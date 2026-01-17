using ABB.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ABB.Infrastructure.Data.Mapping
{
    public class JenisTransaksiMap : IEntityTypeConfiguration<JenisTransaksi>
    {
        public void Configure(EntityTypeBuilder<JenisTransaksi> builder)
        {

            builder.ToTable("abb_jns_transaksi");

            builder.HasKey(t => t.id);

            builder.Property(t => t.id)
                .HasColumnName("id"); 

            builder.Property(t => t.nama)
                .HasColumnName("nama");   

            builder.Property(t => t.kode)
                .HasColumnName("kode");  
        }
    }
}