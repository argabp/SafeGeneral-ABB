using ABB.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ABB.Infrastructure.Data.Mapping
{
    public class KelurahanMap : IEntityTypeConfiguration<Kelurahan>
    {
        public void Configure(EntityTypeBuilder<Kelurahan> builder)
        {
            builder.ToTable("rf07_03", "dbo");
            builder.HasKey(k => new { k.kd_prop, k.kd_kab, k.kd_kec, k.kd_kel });
        }
    }
}