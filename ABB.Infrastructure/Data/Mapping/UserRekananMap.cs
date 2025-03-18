using ABB.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ABB.Infrastructure.Data.Mapping
{
    public class UserRekananMap : IEntityTypeConfiguration<UserRekanan>
    {
        public void Configure(EntityTypeBuilder<UserRekanan> builder)
        {
            builder.ToTable("MS_UserRekanan", "dbo");
            builder.HasKey(k => new { k.kd_cb, k.kd_rk, k.userid });
        }
    }
}