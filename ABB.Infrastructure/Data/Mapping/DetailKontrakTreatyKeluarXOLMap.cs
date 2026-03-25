using ABB.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ABB.Infrastructure.Data.Mapping
{
    public class DetailKontrakTreatyKeluarXOLMap : IEntityTypeConfiguration<DetailKontrakTreatyKeluarXOL>
    {
        public void Configure(EntityTypeBuilder<DetailKontrakTreatyKeluarXOL> builder)
        {
            builder.ToTable("ri02td01", "dbo");
            builder.HasKey(k => new
                { k.kd_cb, k.kd_jns_sor, k.kd_tty_npps, k.kd_grp_pas, k.kd_rk_pas });
        }
    }
}