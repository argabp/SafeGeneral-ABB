using ABB.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ABB.Infrastructure.Data.Mapping
{
    public class TRAkseptasiStatusAttachmentMap : IEntityTypeConfiguration<TRAkseptasiStatusAttachment>
    {
        public void Configure(EntityTypeBuilder<TRAkseptasiStatusAttachment> builder)
        {
            builder.ToTable("TR_AkseptasiStatusAttachment", "dbo");
            builder.HasKey(k => new { k.kd_cb, k.kd_cob, k.kd_scob, k.kd_thn, k.no_aks, k.no_urut, k.kd_dokumen });
        }
    }
}