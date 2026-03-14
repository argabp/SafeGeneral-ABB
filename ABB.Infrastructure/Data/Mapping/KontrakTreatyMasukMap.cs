using ABB.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ABB.Infrastructure.Data.Mapping
{
    public class KontrakTreatyMasukMap : IEntityTypeConfiguration<KontrakTreatyMasuk>
    {
        public void Configure(EntityTypeBuilder<KontrakTreatyMasuk> builder)
        {
            builder.ToTable("ri01i", "dbo");
            builder.HasKey(k => new
                { k.kd_cb, k.kd_jns_sor, k.kd_tty_msk });
        }
    }
}