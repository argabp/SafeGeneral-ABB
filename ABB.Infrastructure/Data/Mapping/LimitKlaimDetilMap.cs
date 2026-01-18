using ABB.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ABB.Infrastructure.Data.Mapping
{
    public class LimitKlaimDetilMap : IEntityTypeConfiguration<LimitKlaimDetil>
    {
        public void Configure(EntityTypeBuilder<LimitKlaimDetil> builder)
        {
            builder.ToTable("MS_LimitKlaimDetil", "dbo");
            builder.HasKey(k => new { k.kd_cb, k.kd_cob, k.kd_scob, k.thn, k.kd_user });
        }
    }
}