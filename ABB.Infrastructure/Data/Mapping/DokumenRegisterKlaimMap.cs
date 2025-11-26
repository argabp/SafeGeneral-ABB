using ABB.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ABB.Infrastructure.Data.Mapping
{
    public class DokumenRegisterKlaimMap : IEntityTypeConfiguration<DokumenRegisterKlaim>
    {
        public void Configure(EntityTypeBuilder<DokumenRegisterKlaim> builder)
        {
            builder.ToTable("cl01d", "dbo");
            builder.HasKey(k => new { k.kd_cb, k.kd_cob, k.kd_scob, k.kd_thn, k.no_kl, k.kd_dok });
        }
    }
}