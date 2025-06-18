using ABB.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ABB.Infrastructure.Data.Mapping
{
    public class DokumenDetilMap : IEntityTypeConfiguration<DokumenDetil>
    {
        public void Configure(EntityTypeBuilder<DokumenDetil> builder)
        {
            builder.ToTable("MS_DokumenDetil", "dbo");
            builder.HasKey(k => k.kd_dokumen);
        }
    }
}