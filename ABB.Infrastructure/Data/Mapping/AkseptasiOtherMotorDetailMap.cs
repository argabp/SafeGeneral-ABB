using ABB.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ABB.Infrastructure.Data.Mapping
{
    public class AkseptasiOtherMotorDetailMap : IEntityTypeConfiguration<AkseptasiOtherMotorDetail>
    {
        public void Configure(EntityTypeBuilder<AkseptasiOtherMotorDetail> builder)
        {
            builder.ToTable("uw04a02d", "dbo");
            builder.HasKey(k => new
                { k.kd_cb, k.kd_cob, k.kd_scob, k.kd_thn, k.no_aks, k.no_updt, k.no_rsk, k.kd_endt, k.thn_ptg_kend });
        }
    }
}