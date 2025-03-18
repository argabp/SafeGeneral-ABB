using ABB.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ABB.Infrastructure.Data.Mapping
{
    public class DetailMataUangMap : IEntityTypeConfiguration<DetailMataUang>
    {
        public void Configure(EntityTypeBuilder<DetailMataUang> builder)
        {
            builder.ToTable("rf06d", "dbo");
            builder.HasKey(k => new { k.kd_mtu, k.tgl_mul, k.tgl_akh });
        }
    }
}