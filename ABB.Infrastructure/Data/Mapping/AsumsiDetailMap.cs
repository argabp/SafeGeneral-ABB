using ABB.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ABB.Infrastructure.Data.Mapping
{
    public class AsumsiDetailMap : IEntityTypeConfiguration<AsumsiDetail>
    {
        public void Configure(EntityTypeBuilder<AsumsiDetail> builder)
        {
            builder.ToTable("MS_AsumsiDetail", "dbo");
            builder.HasKey(k => new { k.KodeAsumsi, k.KodeProduk, k.PeriodeProses, k.Thn });

            builder.Property(p => p.Persentase).HasPrecision(21, 6);
        }
    }
}