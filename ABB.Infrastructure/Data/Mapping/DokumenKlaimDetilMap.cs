using ABB.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ABB.Infrastructure.Data.Mapping
{
    public class DokumenKlaimDetilMap : IEntityTypeConfiguration<DokumenKlaimDetil>
    {
        public void Configure(EntityTypeBuilder<DokumenKlaimDetil> builder)
        {
            builder.ToTable("MS_DokumenKlaimDetil", "dbo");
            builder.HasKey(k => new { k.kd_cob, k.kd_scob, k.kd_dokumen });
        }
    }
}