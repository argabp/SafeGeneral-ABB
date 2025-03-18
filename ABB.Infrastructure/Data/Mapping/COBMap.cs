using ABB.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ABB.Infrastructure.Data.Mapping
{
    public class COBMap : IEntityTypeConfiguration<COB>
    {
        public void Configure(EntityTypeBuilder<COB> builder)
        {
            builder.ToTable("rf04", "dbo");
            builder.HasKey(k => k.kd_cob);
        }
    }
}