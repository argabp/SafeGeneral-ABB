using ABB.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ABB.Infrastructure.Data.Mapping
{
    public class AsumsiMap : IEntityTypeConfiguration<Asumsi>
    {
        public void Configure(EntityTypeBuilder<Asumsi> builder)
        {
            builder.ToTable("MS_Asumsi", "dbo");
            builder.HasKey(k => k.KodeAsumsi);
        }
    }
}