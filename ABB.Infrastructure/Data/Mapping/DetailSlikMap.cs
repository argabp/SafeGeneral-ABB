using ABB.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ABB.Infrastructure.Data.Mapping
{
    public class DetailSlikMap : IEntityTypeConfiguration<DetailSlik>
    {
        public void Configure(EntityTypeBuilder<DetailSlik> builder)
        {
            builder.ToTable("rf03d05", "dbo");
            builder.HasKey(k => new { k.kd_cb, k.kd_grp_rk, k.kd_rk });
        }
    }
}