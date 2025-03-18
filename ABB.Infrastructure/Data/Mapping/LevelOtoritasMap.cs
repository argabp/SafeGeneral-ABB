using ABB.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ABB.Infrastructure.Data.Mapping
{
    public class LevelOtoritasMap : IEntityTypeConfiguration<LevelOtoritas>
    {
        public void Configure(EntityTypeBuilder<LevelOtoritas> builder)
        {
            builder.ToTable("rf41", "dbo");
            builder.HasKey(k => k.kd_user);
        }
    }
}