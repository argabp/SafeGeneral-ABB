using ABB.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ABB.Infrastructure.Data.Mapping
{
    public class LimitAkseptasiDetilMap : IEntityTypeConfiguration<LimitAkseptasiDetil>
    {
        public void Configure(EntityTypeBuilder<LimitAkseptasiDetil> builder)
        {
            builder.ToTable("MS_LimitAkseptasiDetil", "dbo");
            builder.HasKey(k => new { k.kd_cb, k.kd_cob, k.kd_scob, k.thn, k.kd_user });
        }
    }
}