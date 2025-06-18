using ABB.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ABB.Infrastructure.Data.Mapping
{
    public class DokumenAkseptasiDetilMap : IEntityTypeConfiguration<DokumenAkseptasiDetil>
    {
        public void Configure(EntityTypeBuilder<DokumenAkseptasiDetil> builder)
        {
            builder.ToTable("MS_DokumenAkseptasiDetil", "dbo");
            builder.HasKey(k => new { k.kd_cob, k.kd_scob, k.kd_dokumen });
        }
    }
}