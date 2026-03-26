using ABB.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ABB.Infrastructure.Data.Mapping
{
    public class DetailKontrakTreatyKeluarExcludeMap : IEntityTypeConfiguration<DetailKontrakTreatyKeluarExclude>
    {
        public void Configure(EntityTypeBuilder<DetailKontrakTreatyKeluarExclude> builder)
        {
            builder.ToTable("ri01td06", "dbo");
            builder.HasKey(k => new
                { k.kd_cb, k.kd_jns_sor, k.kd_tty_pps, k.kd_okup });
        }
    }
}