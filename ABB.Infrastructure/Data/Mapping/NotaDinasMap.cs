using ABB.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ABB.Infrastructure.Data.Mapping
{
    public class NotaDinasMap : IEntityTypeConfiguration<NotaDinas>
    {
        public void Configure(EntityTypeBuilder<NotaDinas> builder)
        {
            builder.ToTable("TR_NotaDinas", "dbo");
            builder.HasKey(k => k.id_nds);
            
            builder.Property(p => p.prm_bruto).HasPrecision(21, 6);
            builder.Property(p => p.kms).HasPrecision(21, 6);
            builder.Property(p => p.pph).HasPrecision(21, 6);
            builder.Property(p => p.prm_netto).HasPrecision(21, 6);
            builder.Property(p => p.pst_kms).HasPrecision(9, 6);
            builder.Property(p => p.pst_pph).HasPrecision(9, 6);
        }
    }
}