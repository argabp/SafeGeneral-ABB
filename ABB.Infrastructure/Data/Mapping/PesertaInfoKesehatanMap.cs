using ABB.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ABB.Infrastructure.Data.Mapping
{
    public class PesertaInfoKesehatanMap : IEntityTypeConfiguration<PesertaInfoKesehatan>
    {
        public void Configure(EntityTypeBuilder<PesertaInfoKesehatan> builder)
        {
            builder.ToTable("TR_PesertaInfoKesehatan", "dbo");
            builder.HasKey(k => new { k.kd_cb, k.kd_product, k.kd_thn, k.kd_rk, k.no_sppa, k.no_updt });

            builder.Property(p => p.berat_badan).HasPrecision(9, 6);
            builder.Property(p => p.tinggi_badan).HasPrecision(9, 6);
        }
    }
}