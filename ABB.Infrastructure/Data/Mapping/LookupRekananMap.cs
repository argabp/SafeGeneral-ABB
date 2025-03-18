using ABB.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ABB.Infrastructure.Data.Mapping
{
    public class LookupRekananMap : IEntityTypeConfiguration<LookupRekanan>
    {
        public void Configure(EntityTypeBuilder<LookupRekanan> builder)
        {
            builder.ToTable("rf03_ttg", "dbo");
            builder.HasKey(k => new { k.kd_cb, k.kd_rk });
        }
    }
}