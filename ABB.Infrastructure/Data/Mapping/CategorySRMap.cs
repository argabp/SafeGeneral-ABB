using ABB.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ABB.Infrastructure.Data.Mapping
{
    public class CategorySRMap : IEntityTypeConfiguration<CategorySR>
    {
        public void Configure(EntityTypeBuilder<CategorySR> builder)
        {
            builder.ToTable("MS_CategorySR", "dbo");
            builder.HasKey(k => k.kd_kategori);
        }
    }
}