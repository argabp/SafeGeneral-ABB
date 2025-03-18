using ABB.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ABB.Infrastructure.Data.Mapping
{
    public class PeruntukanKendaraanMap : IEntityTypeConfiguration<PeruntukanKendaraan>
    {
        public void Configure(EntityTypeBuilder<PeruntukanKendaraan> builder)
        {
            builder.ToTable("rf16", "dbo");
            builder.HasKey(k => k.kd_utk);
        }
    }
}