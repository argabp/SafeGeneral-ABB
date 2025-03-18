using ABB.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ABB.Infrastructure.Data.Mapping
{
    public class ProvinsiMap : IEntityTypeConfiguration<Provinsi>
    {
        public void Configure(EntityTypeBuilder<Provinsi> builder)
        {
            builder.ToTable("rf07", "dbo");
            builder.HasKey(k => k.kd_prop);
        }
    }
}