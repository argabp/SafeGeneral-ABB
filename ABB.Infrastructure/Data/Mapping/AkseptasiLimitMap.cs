using ABB.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ABB.Infrastructure.Data.Mapping
{
    public class AkseptasiLimitMap : IEntityTypeConfiguration<AkseptasiLimit>
    {
        public void Configure(EntityTypeBuilder<AkseptasiLimit> builder)
        {
            builder.ToTable("TR_AkseptasiLimit", "dbo");
            builder.HasKey(k => new { k.kd_cb, k.kd_cob, k.kd_scob, k.kd_thn, k.no_aks, k.no_urut });
        }
    }
}