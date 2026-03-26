using ABB.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ABB.Infrastructure.Data.Mapping
{
    public class DetailKontrakTreatyKeluarMap : IEntityTypeConfiguration<DetailKontrakTreatyKeluar>
    {
        public void Configure(EntityTypeBuilder<DetailKontrakTreatyKeluar> builder)
        {
            builder.ToTable("ri01td01", "dbo");
            builder.HasKey(k => new
                { k.kd_cb, k.kd_jns_sor, k.kd_tty_pps, k.kd_grp_pas, k.kd_rk_pas });
        }
    }
}