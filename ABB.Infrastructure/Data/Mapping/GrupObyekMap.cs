using ABB.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ABB.Infrastructure.Data.Mapping
{
    public class GrupObyekMap : IEntityTypeConfiguration<GrupObyek>
    {
        public void Configure(EntityTypeBuilder<GrupObyek> builder)
        {
            builder.ToTable("rf14", "dbo");
            builder.HasKey(k => k.kd_grp_oby );
        }
    }
}