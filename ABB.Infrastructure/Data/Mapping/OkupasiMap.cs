using ABB.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ABB.Infrastructure.Data.Mapping
{
    public class OkupasiMap : IEntityTypeConfiguration<Okupasi>
    {
        public void Configure(EntityTypeBuilder<Okupasi> builder)
        {
            builder.ToTable("rf11", "dbo");
            builder.HasKey(k => k.kd_okup);
        }
    }
}