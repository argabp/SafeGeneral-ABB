using ABB.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ABB.Infrastructure.Data.Mapping
{
    public class ApiAuthorizationMap : IEntityTypeConfiguration<ApiAuthorization>
    {
        public void Configure(EntityTypeBuilder<ApiAuthorization> builder)
        {
            builder.ToTable("MS_API_Authorization", "dbo");
            builder.HasKey(k => k.AppId);
        }
    }
}