using ABB.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ABB.Infrastructure.Data.Mapping
{
    public class LokasiResikoMap : IEntityTypeConfiguration<LokasiResiko>
    {
        public void Configure(EntityTypeBuilder<LokasiResiko> builder)
        {
            builder.ToTable("rf25", "dbo");
            builder.HasKey(k => k.kd_pos );
        }
    }
}