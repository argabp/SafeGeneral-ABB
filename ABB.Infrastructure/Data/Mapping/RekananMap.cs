using ABB.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ABB.Infrastructure.Data.Mapping
{
    public class RekananMap : IEntityTypeConfiguration<Rekanan>
    {
        public void Configure(EntityTypeBuilder<Rekanan> builder)
        {
            builder.ToTable("rf03", "dbo");
            builder.HasKey(k => new { k.kd_cb, k.kd_grp_rk, k.kd_rk });
        }
    }
}