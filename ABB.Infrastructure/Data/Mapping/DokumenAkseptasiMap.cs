using ABB.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ABB.Infrastructure.Data.Mapping
{
    public class DokumenAkseptasiMap : IEntityTypeConfiguration<DokumenAkseptasi>
    {
        public void Configure(EntityTypeBuilder<DokumenAkseptasi> builder)
        {
            builder.ToTable("MS_DokumenAkseptasi", "dbo");
            builder.HasKey(k => new { k.kd_cob, k.kd_scob });
        }
    }
}