using ABB.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ABB.Infrastructure.Data.Mapping
{
    public class UserCabangMap : IEntityTypeConfiguration<UserCabang>
    {
        public void Configure(EntityTypeBuilder<UserCabang> builder)
        {
            builder.ToTable("MS_UserCabang", "dbo");
            builder.HasKey(k => new { k.userid, k.kd_cb });
        }
    }
}