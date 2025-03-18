using ABB.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ABB.Infrastructure.Data.Mapping
{
    public class DetailOkupasiMap : IEntityTypeConfiguration<DetailOkupasi>
    {
        public void Configure(EntityTypeBuilder<DetailOkupasi> builder)
        {
            builder.ToTable("rf11d", "dbo");
            builder.HasKey(k => new { k.kd_okup, k.kd_kls_konstr });
        }
    }
}