using ABB.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ABB.Infrastructure.Data.Mapping
{
    public class LookupDetailMap : IEntityTypeConfiguration<LookupDetail>
    {
        public void Configure(EntityTypeBuilder<LookupDetail> builder)
        {
            builder.ToTable("MS_LookupDetail", "dbo");
            builder.HasKey(k => new { k.kd_lookup, k.no_lookup });
        }
    }
}