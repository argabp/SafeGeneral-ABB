using ABB.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ABB.Infrastructure.Data.Mapping
{
    public class LKSMap : IEntityTypeConfiguration<LKS>
    {
        public void Configure(EntityTypeBuilder<LKS> builder)
        {
            builder.ToTable("cl08", "dbo");
            builder.HasKey(k => new { k.kd_cb, k.kd_cob, k.kd_scob, k.kd_thn, k.no_kl, k.no_mts, k.st_tipe_pla, k.no_pla });
        }
    }
}