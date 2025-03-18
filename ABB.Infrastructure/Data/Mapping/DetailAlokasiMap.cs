using ABB.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ABB.Infrastructure.Data.Mapping
{
    public class DetailAlokasiMap : IEntityTypeConfiguration<DetailAlokasi>
    {
        public void Configure(EntityTypeBuilder<DetailAlokasi> builder)
        {
            builder.ToTable("ri02e", "dbo");
            builder.HasKey(k => new { k.kd_cb, k.kd_cob, k.kd_scob, k.kd_thn, 
                k.no_pol, k.no_updt, k.no_rsk, k.kd_endt, k.no_updt_reas, k.kd_jns_sor, 
                k.kd_grp_sor, k.kd_rk_sor, k.kd_grp_sb_bis } );
        }
    }
}