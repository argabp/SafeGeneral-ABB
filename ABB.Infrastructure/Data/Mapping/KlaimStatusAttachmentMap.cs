using ABB.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ABB.Infrastructure.Data.Mapping
{
    public class KlaimStatusAttachmentMap : IEntityTypeConfiguration<KlaimStatusAttachment>
    {
        public void Configure(EntityTypeBuilder<KlaimStatusAttachment> builder)
        {
            builder.ToTable("TR_KlaimStatusAttachment", "dbo");
            builder.HasKey(k => new { k.kd_cb, k.kd_cob, k.kd_scob, k.kd_thn, k.no_kl, k.no_mts, k.no_urut, k.no_dokumen });
        }
    }
}