using ABB.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ABB.Infrastructure.Data.Mapping
{
    public class PertanggunganKendaraanMap : IEntityTypeConfiguration<PertanggunganKendaraan>
    {
        public void Configure(EntityTypeBuilder<PertanggunganKendaraan> builder)
        {
            builder.ToTable("dp01", "dbo");
            builder.HasKey(k => new { k.kd_cob, k.kd_scob, k.kd_jns_ptg });
        }
    }
}