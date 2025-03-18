using ABB.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ABB.Infrastructure.Data.Mapping
{
    public class CabangMap : IEntityTypeConfiguration<Cabang>
    {
        public void Configure(EntityTypeBuilder<Cabang> builder)
        {
            builder.ToTable("rf01", "dbo");
            builder.HasKey(k => k.kd_cb);
        }
    }
}