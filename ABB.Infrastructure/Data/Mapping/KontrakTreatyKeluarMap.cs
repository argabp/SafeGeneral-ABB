using ABB.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ABB.Infrastructure.Data.Mapping
{
    public class KontrakTreatyKeluarMap : IEntityTypeConfiguration<KontrakTreatyKeluar>
    {
        public void Configure(EntityTypeBuilder<KontrakTreatyKeluar> builder)
        {
            builder.ToTable("ri01t", "dbo");
            builder.HasKey(k => new
                { k.kd_cb, k.kd_jns_sor, k.kd_tty_pps });
        }
    }
}