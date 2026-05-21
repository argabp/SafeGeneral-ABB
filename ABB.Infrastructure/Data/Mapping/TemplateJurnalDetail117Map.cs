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
            
            // PERBAIKAN: Ubah t.event menjadi t.Event
            builder.HasKey(t => new { t.type_tr, t.type_jr, t.metode, t.Event, t.jn_ass, t.gl_akun});

            builder.Property(t => t.type_tr).HasColumnName("type_tr");
            builder.Property(t => t.type_jr).HasColumnName("type_jr");
            builder.Property(t => t.metode).HasColumnName("metode");
            builder.Property(t => t.Event).HasColumnName("event");
            builder.Property(t => t.jn_ass).HasColumnName("jn_ass");
            builder.Property(t => t.gl_akun).HasColumnName("gl_akun");
            builder.Property(t => t.gl_rumus).HasColumnName("gl_rumus");
            builder.Property(t => t.gl_dk).HasColumnName("gl_dk");
            builder.Property(t => t.gl_urut).HasColumnName("gl_urut");
            builder.Property(t => t.flag_detail).HasColumnName("flag_detail");
            builder.Property(t => t.flag_nt).HasColumnName("flag_nt");
        }
    }
}