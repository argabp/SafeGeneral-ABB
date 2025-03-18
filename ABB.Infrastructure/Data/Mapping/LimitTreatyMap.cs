using ABB.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ABB.Infrastructure.Data.Mapping
{
    public class LimitTreatyMap : IEntityTypeConfiguration<LimitTreaty>
    {
        public void Configure(EntityTypeBuilder<LimitTreaty> builder)
        {
            builder.ToTable("rf48", "dbo");
            builder.HasKey(k => new {  k.kd_cob, k.kd_tol });
        }
    }
}