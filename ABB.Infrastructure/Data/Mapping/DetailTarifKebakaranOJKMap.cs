using ABB.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ABB.Infrastructure.Data.Mapping
{
    public class DetailTarifKebakaranOJKMap : IEntityTypeConfiguration<DetailTarifKebakaranOJK>
    {
        public void Configure(EntityTypeBuilder<DetailTarifKebakaranOJK> builder)
        {
            builder.ToTable("rf11d01", "dbo");
            builder.HasKey(k => new { k.kd_okup, k.kd_kls_konstr });
        }
    }
}