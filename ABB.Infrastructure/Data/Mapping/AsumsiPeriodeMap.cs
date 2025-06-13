using ABB.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ABB.Infrastructure.Data.Mapping
{
    public class AsumsiPeriodeMap : IEntityTypeConfiguration<AsumsiPeriode>
    {
        public void Configure(EntityTypeBuilder<AsumsiPeriode> builder)
        {
            builder.ToTable("MS_AsumsiPeriode", "dbo");
            builder.HasKey(k => new { k.KodeAsumsi, k.KodeProduk, k.PeriodeProses });
        }
    }
}