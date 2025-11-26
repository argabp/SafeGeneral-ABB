using ABB.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ABB.Infrastructure.Data.Mapping
{
    public class TypeCoaMap : IEntityTypeConfiguration<TypeCoa>
    {
        public void Configure(EntityTypeBuilder<TypeCoa> builder)
        {
            builder.ToTable("abb_tipe_coa");

            // Tentukan gl_kode sebagai Primary Key
            builder.HasKey(tc => tc.Type); 
            
            builder.Property(tc => tc.Nama).HasColumnName("f_nama");
            builder.Property(tc => tc.Pos).HasColumnName("gl_pos");
            builder.Property(tc => tc.Dk).HasColumnName("gl_dk");
            builder.Property(tc => tc.Type).HasColumnName("gl_type");
        }
    }
}