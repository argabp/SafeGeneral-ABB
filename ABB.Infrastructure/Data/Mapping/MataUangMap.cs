using ABB.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ABB.Infrastructure.Data.Mapping
{
    public class MataUangMap : IEntityTypeConfiguration<MataUang>
    {
        public void Configure(EntityTypeBuilder<MataUang> builder)
        {
            builder.ToTable("rf06", "dbo");
            builder.HasKey(k => k.kd_mtu);
        }
    }
}