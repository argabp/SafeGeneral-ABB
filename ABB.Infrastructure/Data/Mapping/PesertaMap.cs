using ABB.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ABB.Infrastructure.Data.Mapping
{
    public class PesertaMap : IEntityTypeConfiguration<Peserta>
    {
        public void Configure(EntityTypeBuilder<Peserta> builder)
        {
            builder.ToTable("TR_Peserta", "dbo");
            builder.HasKey(k => new { k.kd_cb, k.kd_product, k.kd_thn, k.kd_rk, k.no_sppa, k.no_updt });
        }
    }
}