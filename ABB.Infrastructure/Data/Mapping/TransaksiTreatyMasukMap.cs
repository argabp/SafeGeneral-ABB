using ABB.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ABB.Infrastructure.Data.Mapping
{
    public class TransaksiTreatyMasukMap : IEntityTypeConfiguration<TransaksiTreatyMasuk>
    {
        public void Configure(EntityTypeBuilder<TransaksiTreatyMasuk> builder)
        {
            builder.ToTable("ri02i", "dbo");
            builder.HasKey(k => new { k.kd_cb, k.kd_thn, k.kd_bln, k.kd_jns_sor, k.kd_tty_msk, k.kd_mtu, k.no_tr });
        }
    }
}