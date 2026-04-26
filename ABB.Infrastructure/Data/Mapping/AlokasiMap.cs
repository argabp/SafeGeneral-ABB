using ABB.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ABB.Infrastructure.Data.Mapping
{
    public class AlokasiMap : IEntityTypeConfiguration<Alokasi>
    {
        public void Configure(EntityTypeBuilder<Alokasi> builder)
        {
            builder.ToTable("ri01e", "dbo");
            builder.HasKey(k => new { k.kd_cb, k.kd_cob, k.kd_scob, k.kd_thn, 
                k.no_pol, k.no_updt, k.no_rsk, k.kd_endt, k.no_updt_reas} );
        }
    }
}