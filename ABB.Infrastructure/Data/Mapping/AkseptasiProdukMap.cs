using ABB.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ABB.Infrastructure.Data.Mapping
{
    public class AkseptasiProdukMap : IEntityTypeConfiguration<AkseptasiProduk>
    {
        public void Configure(EntityTypeBuilder<AkseptasiProduk> builder)
        {
            builder.ToTable("MS_AkseptasiProduk", "dbo");
            builder.HasKey(k => new { k.kd_cob, k.kd_scob });
        }
    }
}