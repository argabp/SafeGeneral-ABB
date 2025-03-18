using ABB.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ABB.Infrastructure.Data.Mapping
{
    public class DetailGrupResikoMap : IEntityTypeConfiguration<DetailGrupResiko>
    {
        public void Configure(EntityTypeBuilder<DetailGrupResiko> builder)
        {
            builder.ToTable("rf10d", "dbo");
            builder.HasKey(k => new { k.kd_grp_rsk, k.kd_rsk } );
        }
    }
}