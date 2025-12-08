using ABB.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ABB.Infrastructure.Data.Mapping
{
    public class TemplateJurnalDetail117Map : IEntityTypeConfiguration<TemplateJurnalDetail117>
    {
        public void Configure(EntityTypeBuilder<TemplateJurnalDetail117> builder)
        {
            
            builder.ToTable("abb_templatejurnaldetail117");
            
            // ditambahin untuk pk
            builder.HasKey(t => t.Type);
            builder.Property(t => t.Type).HasColumnName("type");
            builder.Property(t => t.JenisAss).HasColumnName("jn_ass");
            builder.Property(t => t.GlAkun).HasColumnName("gl_akun");
            builder.Property(t => t.GlRumus).HasColumnName("gl_rumus");
            builder.Property(t => t.GlDk).HasColumnName("gl_dk");
            builder.Property(t => t.GlUrut).HasColumnName("gl_urut");
            builder.Property(t => t.FlagDetail).HasColumnName("flag_detail");
            builder.Property(t => t.FlagNt).HasColumnName("flag_nt");
        }
    }
}