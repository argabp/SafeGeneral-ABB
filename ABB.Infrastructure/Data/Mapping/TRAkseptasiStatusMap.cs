using ABB.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ABB.Infrastructure.Data.Mapping
{
    public class TRAkseptasiStatusMap : IEntityTypeConfiguration<TRAkseptasiStatus>
    {
        public void Configure(EntityTypeBuilder<TRAkseptasiStatus> builder)
        {
            builder.ToTable("TR_AkseptasiStatus", "dbo");
            builder.HasKey(k => new { k.kd_cb, k.kd_cob, k.kd_scob, k.kd_thn, k.no_aks, k.no_urut });
        }
    }
}