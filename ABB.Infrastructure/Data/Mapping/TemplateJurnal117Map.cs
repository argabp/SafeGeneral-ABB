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

            // PERBAIKAN: Ubah t.event menjadi t.Event
            builder.HasKey(t => new { t.type_tr, t.type_jr, t.metode, t.Event, t.jn_ass });

            builder.Property(t => t.type_tr)
                .HasColumnName("type_tr");   

            builder.Property(t => t.type_jr)
                .HasColumnName("type_jr");
                
            builder.Property(t => t.metode)
                .HasColumnName("metode"); 

            // Pastikan di file entity (TemplateJurnal117.cs), propertinya ditulis "public string Event { get; set; }"
            builder.Property(t => t.Event)
                .HasColumnName("event");

            builder.Property(t => t.jn_ass)
                .HasColumnName("jn_ass"); 

            builder.Property(t => t.nm_jr)
                .HasColumnName("nm_jr"); 
        }
    }
}