using ABB.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ABB.Infrastructure.Data.Mapping
{
    public class NotaDinasStatusDokumenMap : IEntityTypeConfiguration<NotaDinasStatusDokumen>
    {
        public void Configure(EntityTypeBuilder<NotaDinasStatusDokumen> builder)
        {
            builder.ToTable("TR_NotaDinasStatusDokumen", "dbo");
            builder.HasKey(k => new { k.id_nds, k.no_urut, k.no_dokumen});
        }
    }
}