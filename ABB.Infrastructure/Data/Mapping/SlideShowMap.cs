using ABB.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ABB.Infrastructure.Data.Mapping
{
    public class SlideShowMap : IEntityTypeConfiguration<SlideShow>
    {
        public void Configure(EntityTypeBuilder<SlideShow> builder)
        {
            builder.ToTable("MS_SlideShow", "dbo");
            builder.HasKey(k => k.Id);
        }
    }
}