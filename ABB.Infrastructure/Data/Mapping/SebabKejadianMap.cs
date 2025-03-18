using ABB.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ABB.Infrastructure.Data.Mapping
{
    public class SebabKejadianMap : IEntityTypeConfiguration<SebabKejadian>
    {
        public void Configure(EntityTypeBuilder<SebabKejadian> builder)
        {
            builder.ToTable("rf34", "dbo");
            builder.HasKey(k => new { k.kd_cob , k.kd_sebab});
        }
    }
}