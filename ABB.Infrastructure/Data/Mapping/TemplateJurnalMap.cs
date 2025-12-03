using ABB.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ABB.Infrastructure.Data.Mapping
{
    public class TemplateJurnal62Map : IEntityTypeConfiguration<TemplateJurnal62>
    {
        public void Configure(EntityTypeBuilder<TemplateJurnal62> builder)
        {
            
            builder.ToTable("abb_templatejurnal62");
            
            // ditambahin untuk pk
            builder.HasKey(t => t.Type);
            builder.Property(t => t.Type).HasColumnName("type");
            builder.Property(t => t.JenisAss).HasColumnName("jn_ass");
            builder.Property(t => t.NamaJurnal).HasColumnName("nm_jurnal");
        }
    }
}