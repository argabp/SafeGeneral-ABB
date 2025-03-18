using ABB.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ABB.Infrastructure.Data.Mapping
{
    public class ObligeeMap : IEntityTypeConfiguration<Obligee>
    {
        public void Configure(EntityTypeBuilder<Obligee> builder)
        {
            builder.ToTable("rf47", "dbo");
            builder.HasKey(k => new { k.kd_cb, k.kd_grp_rk, k.kd_rk });
        }
    }
}