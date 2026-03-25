using ABB.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ABB.Infrastructure.Data.Mapping
{
    public class KontrakTreatyKeluarXOLMap : IEntityTypeConfiguration<KontrakTreatyKeluarXOL>
    {
        public void Configure(EntityTypeBuilder<KontrakTreatyKeluarXOL> builder)
        {
            builder.ToTable("ri02t", "dbo");
            builder.HasKey(k => new
                { k.kd_cb, k.kd_jns_sor, k.kd_tty_npps });
        }
    }
}