using ABB.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ABB.Infrastructure.Data.Mapping
{
    public class PeriodeProsesMap : IEntityTypeConfiguration<PeriodeProsesModel>
    {
        public void Configure(EntityTypeBuilder<PeriodeProsesModel> builder)
        {
            builder.ToTable("MS_PeriodeProses", "dbo");
            builder.HasKey(k => k.PeriodeProses);
        }
    }
}