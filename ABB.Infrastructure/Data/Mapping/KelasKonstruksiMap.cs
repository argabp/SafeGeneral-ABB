using ABB.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ABB.Infrastructure.Data.Mapping
{
    public class KelasKonstruksiMap : IEntityTypeConfiguration<KelasKonstruksi>
    {
        public void Configure(EntityTypeBuilder<KelasKonstruksi> builder)
        {
            builder.ToTable("rf13", "dbo");
            builder.HasKey(k => k.kd_kls_konstr);
        }
    }
}