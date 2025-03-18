using ABB.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ABB.Infrastructure.Data.Mapping
{
    public class KodeKonfirmasiMap : IEntityTypeConfiguration<KodeKonfirmasi>
    {
        public void Configure(EntityTypeBuilder<KodeKonfirmasi> builder)
        {
            builder.ToTable("rf42", "dbo");
            builder.HasKey(k => new
                { k.kd_cb, k.kd_cob, k.kd_scob, k.kd_thn, k.no_aks, k.kd_konfirm });
        }
    }
}