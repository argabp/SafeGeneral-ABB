using ABB.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ABB.Infrastructure.Data.Mapping
{
    public class KapasitasCabangMap : IEntityTypeConfiguration<KapasitasCabang>
    {
        public void Configure(EntityTypeBuilder<KapasitasCabang> builder)
        {
            builder.ToTable("rf43", "dbo");
            builder.HasKey(k => new { k.kd_cb, k.kd_cob, k.kd_scob, k.thn });

            builder.Property(p => p.nilai_kapasitas).HasPrecision(21, 6);
            builder.Property(p => p.nilai_kl).HasPrecision(21, 6);
        }
    }
}