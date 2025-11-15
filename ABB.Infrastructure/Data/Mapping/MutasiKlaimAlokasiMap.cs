using ABB.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ABB.Infrastructure.Data.Mapping
{
    public class MutasiKlaimAlokasiMap : IEntityTypeConfiguration<MutasiKlaimAlokasi>
    {
        public void Configure(EntityTypeBuilder<MutasiKlaimAlokasi> builder)
        {
            builder.ToTable("cl05", "dbo");
            builder.HasKey(k => new { k.kd_cb, k.kd_cob, k.kd_scob, 
                k.kd_thn, k.no_kl, k.no_mts, k.kd_grp_pas, k.kd_rk_pas });
        }
    }
}