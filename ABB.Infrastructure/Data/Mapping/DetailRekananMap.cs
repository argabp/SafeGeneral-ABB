using ABB.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ABB.Infrastructure.Data.Mapping
{
    public class DetailRekananMap : IEntityTypeConfiguration<DetailRekanan>
    {
        public void Configure(EntityTypeBuilder<DetailRekanan> builder)
        {
            builder.ToTable("rf03d", "dbo");
            builder.HasKey(k => new { k.kd_cb, k.kd_grp_rk, k.kd_rk });
        }
    }
}