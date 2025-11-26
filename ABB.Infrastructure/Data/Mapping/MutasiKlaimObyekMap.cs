using ABB.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ABB.Infrastructure.Data.Mapping
{
    public class MutasiKlaimObyekMap : IEntityTypeConfiguration<MutasiKlaimObyek>
    {
        public void Configure(EntityTypeBuilder<MutasiKlaimObyek> builder)
        {
            builder.ToTable("cl03d", "dbo");
            builder.HasKey(k => new { k.kd_cb, k.kd_cob, k.kd_scob, k.kd_thn, k.no_kl, k.no_mts, k.no_oby });
        }
    }
}