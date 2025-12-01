using ABB.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ABB.Infrastructure.Data.Mapping
{
    public class Coa117Map : IEntityTypeConfiguration<Coa117>
    {
        public void Configure(EntityTypeBuilder<Coa117> builder)
        {
            builder.ToTable("abb_coa117");

            // Tentukan gl_kode sebagai Primary Key
            builder.HasKey(c => c.gl_kode); 
            
            builder.Property(c => c.gl_kode).HasColumnName("gl_kode");
            builder.Property(c => c.gl_nama).HasColumnName("gl_nama");
            builder.Property(c => c.gl_dept).HasColumnName("gl_dept");
            builder.Property(c => c.gl_type).HasColumnName("gl_type");
        }
    }
}