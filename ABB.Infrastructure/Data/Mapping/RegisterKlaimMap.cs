using ABB.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ABB.Infrastructure.Data.Mapping
{
    public class RegisterKlaimMap : IEntityTypeConfiguration<RegisterKlaim>
    {
        public void Configure(EntityTypeBuilder<RegisterKlaim> builder)
        {
            builder.ToTable("cl01", "dbo");
            builder.HasKey(k => new { k.kd_cb, k.kd_cob, k.kd_scob, k.kd_thn, k.no_kl });
        }
    }
}