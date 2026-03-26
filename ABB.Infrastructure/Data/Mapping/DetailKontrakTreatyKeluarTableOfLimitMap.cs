using ABB.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ABB.Infrastructure.Data.Mapping
{
    public class DetailKontrakTreatyKeluarTableOfLimitMap : IEntityTypeConfiguration<DetailKontrakTreatyKeluarTableOfLimit>
    {
        public void Configure(EntityTypeBuilder<DetailKontrakTreatyKeluarTableOfLimit> builder)
        {
            builder.ToTable("ri01td04", "dbo");
            builder.HasKey(k => new
                { k.kd_cb, k.kd_jns_sor, k.kd_tty_pps, k.kd_okup, k.category_rsk, k.kd_kls_konstr });
        }
    }
}