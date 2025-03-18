using ABB.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ABB.Infrastructure.Data.Mapping
{
    public class AkuisisiMap : IEntityTypeConfiguration<Akuisisi>
    {
        public void Configure(EntityTypeBuilder<Akuisisi> builder)
        {
            builder.ToTable("rf40", "dbo");
            builder.HasKey(k => new { k.kd_mtu, k.kd_cob, k.kd_scob, k.kd_thn });

            builder.Property(p => p.nilai_maks_acq).HasPrecision(21, 6);
            builder.Property(p => p.nilai_min_acq).HasPrecision(21, 6);
            builder.Property(p => p.nilai_acq).HasPrecision(21, 6);
        }
    }
}