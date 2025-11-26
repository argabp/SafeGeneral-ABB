using ABB.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ABB.Infrastructure.Data.Mapping
{
    public class AnalisaDanEvaluasiMap : IEntityTypeConfiguration<AnalisaDanEvaluasi>
    {
        public void Configure(EntityTypeBuilder<AnalisaDanEvaluasi> builder)
        {
            builder.ToTable("cl01d03", "dbo");
            builder.HasKey(k => new { k.kd_cb, k.kd_cob, k.kd_scob, k.kd_thn, k.no_kl });
        }
    }
}