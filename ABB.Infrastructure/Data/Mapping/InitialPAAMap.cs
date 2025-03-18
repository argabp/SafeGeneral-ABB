using ABB.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ABB.Infrastructure.Data.Mapping
{
    public class InitialPAAMap : IEntityTypeConfiguration<InitialPAA>
    {
        public void Configure(EntityTypeBuilder<InitialPAA> builder)
        {
            builder.ToTable("TR_IntialPAA", "dbo");
            builder.HasKey(k => new { k.Id, k.PeriodeProses });
            
            builder.Property(p => p.LRC).HasPrecision(21, 6);
            builder.Property(p => p.LRCIDR).HasPrecision(21, 6);
        }
    }
}