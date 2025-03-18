using ABB.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ABB.Infrastructure.Data.Mapping
{
    public class PesertaStatusAllMap : IEntityTypeConfiguration<PesertaStatusAll>
    {
        public void Configure(EntityTypeBuilder<PesertaStatusAll> builder)
        {
            builder.ToTable("TR_PesertaStatus1", "dbo");
            builder.HasKey(k => new { k.kd_cb, k.kd_product, k.kd_thn, k.kd_rk, k.no_sppa, k.no_updt, k.no_urut });
        }
    }
}