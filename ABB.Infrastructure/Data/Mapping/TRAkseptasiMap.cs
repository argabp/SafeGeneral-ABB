using ABB.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ABB.Infrastructure.Data.Mapping
{
    public class TRAkseptasiMap : IEntityTypeConfiguration<TRAkseptasi>
    {
        public void Configure(EntityTypeBuilder<TRAkseptasi> builder)
        {
            builder.ToTable("TR_Akseptasi", "dbo");
            builder.HasKey(k => new { k.kd_cb, k.kd_cob, k.kd_scob, k.kd_thn, k.no_aks });
        }
    }
}