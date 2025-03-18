using ABB.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ABB.Infrastructure.Data.Mapping
{
    public class ApiAuditTrailMap : IEntityTypeConfiguration<ApiAuditTrail>
    {
        public void Configure(EntityTypeBuilder<ApiAuditTrail> builder)
        {
            builder.ToTable("TR_ApiAuditTrail", "dbo");
            builder.HasKey(k => k.Id);
        }
    }
}