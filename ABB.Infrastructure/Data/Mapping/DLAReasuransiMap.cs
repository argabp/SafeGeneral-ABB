using ABB.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ABB.Infrastructure.Data.Mapping
{
    public class DLAReasuransiMap : IEntityTypeConfiguration<DLAReasuransi>
    {
        public void Configure(EntityTypeBuilder<DLAReasuransi> builder)
        {
            builder.ToTable("cl09r", "dbo");
            builder.HasKey(k => new { k.kd_cb, k.kd_cob, k.kd_scob, k.kd_thn, k.no_kl, k.no_mts, k.no_dla });
        }
    }
}