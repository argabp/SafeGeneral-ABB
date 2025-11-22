using ABB.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ABB.Infrastructure.Data.Mapping
{
    public class LKTMap : IEntityTypeConfiguration<LKT>
    {
        public void Configure(EntityTypeBuilder<LKT> builder)
        {
            builder.ToTable("cl09", "dbo");
            builder.HasKey(k => new { k.kd_cb, k.kd_cob, k.kd_scob, k.kd_thn, k.no_kl, k.no_mts, k.st_tipe_dla, k.no_dla });
        }
    }
}