using ABB.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ABB.Infrastructure.Data.Mapping
{
    public class DetailLokasiResikoMap : IEntityTypeConfiguration<DetailLokasiResiko>
    {
        public void Configure(EntityTypeBuilder<DetailLokasiResiko> builder)
        {
            builder.ToTable("rf25d", "dbo");
            builder.HasKey(k => new { k.kd_pos, k.kd_lok_rsk } );
        }
    }
}