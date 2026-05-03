using ABB.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ABB.Infrastructure.Data.Mapping
{
    public class ProsesPremiXOLKeluarMap : IEntityTypeConfiguration<ProsesPremiXOLKeluar>
    {
        public void Configure(EntityTypeBuilder<ProsesPremiXOLKeluar> builder)
        {
            builder.ToTable("ri03t", "dbo");
            builder.HasKey(k => new { k.kd_cb, k.kd_thn, k.kd_bln, k.kd_jns_sor, k.kd_tty_npps, k.kd_mtu, k.no_tr });
        }
    }
}