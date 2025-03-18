using ABB.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ABB.Infrastructure.Data.Mapping
{
    public class AkseptasiOtherCargoDetailMap : IEntityTypeConfiguration<AkseptasiOtherCargoDetail>
    {
        public void Configure(EntityTypeBuilder<AkseptasiOtherCargoDetail> builder)
        {
            builder.ToTable("uw04a03d", "dbo");
            builder.HasKey(k => new { k.kd_cb, k.kd_cob, k.kd_scob, k.kd_thn, 
                k.no_aks, k.no_updt, k.no_rsk, k.kd_endt, k.no_urut });
        }
    }
}