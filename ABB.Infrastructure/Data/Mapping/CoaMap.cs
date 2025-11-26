using ABB.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ABB.Infrastructure.Data.Mapping
{
    public class CoaMap : IEntityTypeConfiguration<Coa>
    {
        public void Configure(EntityTypeBuilder<Coa> builder)
        {
            builder.ToTable("abb_coa");

            // Tentukan gl_kode sebagai Primary Key
            builder.HasKey(c => c.gl_kode); 
            
            builder.Property(c => c.gl_kode).HasColumnName("gl_kode");
            builder.Property(c => c.gl_nama).HasColumnName("gl_nama");
            builder.Property(c => c.gl_dept).HasColumnName("gl_dept");
            builder.Property(c => c.gl_type).HasColumnName("gl_type");
        }
    }
}