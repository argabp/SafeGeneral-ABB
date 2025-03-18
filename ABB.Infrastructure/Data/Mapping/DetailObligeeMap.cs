using ABB.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ABB.Infrastructure.Data.Mapping
{
    public class DetailObligeeMap : IEntityTypeConfiguration<DetailObligee>
    {
        public void Configure(EntityTypeBuilder<DetailObligee> builder)
        {
            builder.ToTable("rf47d", "dbo");
            builder.HasKey(k => new { k.kd_cb, k.kd_grp_rk, k.kd_rk });
        }
    }
}