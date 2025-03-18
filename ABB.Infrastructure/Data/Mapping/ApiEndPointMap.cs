using ABB.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ABB.Infrastructure.Data.Mapping
{
    public class ApiEndPointMap : IEntityTypeConfiguration<ApiEndPoint>
    {
        public void Configure(EntityTypeBuilder<ApiEndPoint> builder)
        {
            builder.ToTable("MS_ApiEndPoint", "dbo");
            builder.HasKey(k => k.Id);
        }
    }
}