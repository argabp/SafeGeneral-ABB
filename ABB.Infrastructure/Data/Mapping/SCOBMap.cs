using ABB.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ABB.Infrastructure.Data.Mapping
{
    public class SCOBMap : IEntityTypeConfiguration<SCOB>
    {
        public void Configure(EntityTypeBuilder<SCOB> builder)
        {
            builder.ToTable("rf05", "dbo");
            builder.HasKey(k => new { k.kd_cob , k.kd_scob});
        }
    }
}