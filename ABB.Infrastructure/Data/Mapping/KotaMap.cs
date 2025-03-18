using ABB.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ABB.Infrastructure.Data.Mapping
{
    public class KotaMap : IEntityTypeConfiguration<Kota>
    {
        public void Configure(EntityTypeBuilder<Kota> builder)
        {
            builder.ToTable("rf07_00", "dbo");
            builder.HasKey(k => k.kd_kota);
        }
    }
}