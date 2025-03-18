using ABB.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ABB.Infrastructure.Data.Mapping
{
    public class KabupatenMap : IEntityTypeConfiguration<Kabupaten>
    {
        public void Configure(EntityTypeBuilder<Kabupaten> builder)
        {
            builder.ToTable("rf07_01", "dbo");
            builder.HasKey(k => new { k.kd_prop, k.kd_kab });
        }
    }
}