using ABB.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ABB.Infrastructure.Data.Mapping
{
    public class TemplateJurnal117Map : IEntityTypeConfiguration<TemplateJurnal117>
    {
        public void Configure(EntityTypeBuilder<TemplateJurnal117> builder)
        {
            builder.ToTable("abb_templatejurnal117");

            builder.HasKey(t => new { t.Type, t.JenisAss });

            builder.Property(t => t.Type)
                .HasColumnName("type")
                .HasMaxLength(2);   // sesuaikan dengan DB, jika 1 → ganti 1

            builder.Property(t => t.JenisAss)
                .HasColumnName("jn_ass")
                .HasMaxLength(2);    // WAJIB, karena DB hanya 2 karakter

            builder.Property(t => t.NamaJurnal)
                .HasColumnName("nm_jurnal")
                .HasMaxLength(50);   // sesuaikan ukuran DB
        }
    }
}