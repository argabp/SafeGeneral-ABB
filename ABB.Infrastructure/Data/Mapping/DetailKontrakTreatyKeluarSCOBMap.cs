using ABB.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ABB.Infrastructure.Data.Mapping
{
    public class DetailKontrakTreatyKeluarSCOBMap : IEntityTypeConfiguration<DetailKontrakTreatyKeluarSCOB>
    {
        public void Configure(EntityTypeBuilder<DetailKontrakTreatyKeluarSCOB> builder)
        {
            builder.ToTable("ri01td02", "dbo");
            builder.HasKey(k => new
                { k.kd_cb, k.kd_jns_sor, k.kd_tty_pps, k.kd_cob, k.kd_scob });
        }
    }
}