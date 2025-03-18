using ABB.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ABB.Infrastructure.Data.Mapping
{
    public class KapalMap : IEntityTypeConfiguration<Kapal>
    {
        public void Configure(EntityTypeBuilder<Kapal> builder)
        {
            builder.ToTable("rf30", "dbo");
            builder.HasKey(k => k.kd_kapal);
        }
    }
}