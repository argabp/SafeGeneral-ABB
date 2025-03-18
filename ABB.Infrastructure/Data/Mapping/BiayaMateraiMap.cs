using ABB.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ABB.Infrastructure.Data.Mapping
{
    public class BiayaMateraiMap : IEntityTypeConfiguration<BiayaMaterai>
    {
        public void Configure(EntityTypeBuilder<BiayaMaterai> builder)
        {
            builder.ToTable("dp02", "dbo");
            builder.HasKey(k => new { k.kd_mtu, k.nilai_prm_mul, k.nilai_prm_akh });

            builder.Property(p => p.nilai_prm_akh).HasPrecision(21, 6);
            builder.Property(p => p.nilai_prm_mul).HasPrecision(21, 6);
            builder.Property(p => p.nilai_bia_mat).HasPrecision(21, 6);
        }
    }
}