using ABB.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ABB.Infrastructure.Data.Mapping
{
    public class MutasiKlaimBebanMap : IEntityTypeConfiguration<MutasiKlaimBeban>
    {
        public void Configure(EntityTypeBuilder<MutasiKlaimBeban> builder)
        {
            builder.ToTable("cl04", "dbo");
            builder.HasKey(k => new { k.kd_cb, k.kd_cob, k.kd_scob, k.kd_thn, k.no_kl, k.no_mts, k.no_urut });
        }
    }
}