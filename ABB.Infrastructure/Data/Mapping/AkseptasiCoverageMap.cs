using ABB.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ABB.Infrastructure.Data.Mapping
{
    public class AkseptasiCoverageMap : IEntityTypeConfiguration<AkseptasiCoverage>
    {
        public void Configure(EntityTypeBuilder<AkseptasiCoverage> builder)
        {
            builder.ToTable("uw05a", "dbo");
            builder.HasKey(k => new { k.kd_cb, k.kd_cob, k.kd_scob, k.kd_thn, k.no_aks, k.no_updt, k.no_rsk, k.kd_endt, k.kd_cvrg });

            builder.Property(p => p.pst_rate_prm).HasPrecision(9, 6);
            builder.Property(p => p.pst_dis).HasPrecision(9, 6);
            builder.Property(p => p.pst_kms).HasPrecision(9, 6);
        }
    }
}