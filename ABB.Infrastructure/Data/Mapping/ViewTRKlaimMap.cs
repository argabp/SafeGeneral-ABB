using ABB.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ABB.Infrastructure.Data.Mapping
{
    public class ViewTRKlaimMap : IEntityTypeConfiguration<ViewTrKlaim>
    {
        public void Configure(EntityTypeBuilder<ViewTrKlaim> builder)
        {
            builder.ToTable("v_tr_klaim", "dbo");
            builder.HasKey(k => new { k.kd_cb, k.kd_cob, k.kd_scob, k.kd_thn, k.no_kl, k.no_mts });
        }
    }
}