using ABB.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ABB.Infrastructure.Data.Mapping
{
    public class LabaRugiKursMap : IEntityTypeConfiguration<LabaRugiKurs>
    {
        public void Configure(EntityTypeBuilder<LabaRugiKurs> builder)
        {
            builder.ToTable("abb_kodeakun_valas");

            // PK ganti ke ID
            builder.HasKey(t => t.Id);

            builder.Property(t => t.Id).HasColumnName("id");
            builder.Property(t => t.gl_kode).HasColumnName("gl_kode");
            builder.Property(t => t.gl_dept).HasColumnName("gl_dept");
            builder.Property(t => t.gl_nama).HasColumnName("gl_nama");
        }
    }
}