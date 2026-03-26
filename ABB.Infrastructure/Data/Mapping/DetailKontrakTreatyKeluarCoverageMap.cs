using ABB.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ABB.Infrastructure.Data.Mapping
{
    public class DetailKontrakTreatyKeluarCoverageMap : IEntityTypeConfiguration<DetailKontrakTreatyKeluarCoverage>
    {
        public void Configure(EntityTypeBuilder<DetailKontrakTreatyKeluarCoverage> builder)
        {
            builder.ToTable("ri01td03", "dbo");
            builder.HasKey(k => new
                { k.kd_cb, k.kd_jns_sor, k.kd_tty_pps, k.kd_cvrg });
        }
    }
}