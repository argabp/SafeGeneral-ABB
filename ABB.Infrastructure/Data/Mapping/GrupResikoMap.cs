using ABB.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ABB.Infrastructure.Data.Mapping
{
    public class GrupResikoMap : IEntityTypeConfiguration<GrupResiko>
    {
        public void Configure(EntityTypeBuilder<GrupResiko> builder)
        {
            builder.ToTable("rf10", "dbo");
            builder.HasKey(k => k.kd_grp_rsk );
        }
    }
}