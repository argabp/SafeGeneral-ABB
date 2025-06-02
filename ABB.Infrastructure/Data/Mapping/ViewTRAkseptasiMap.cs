using ABB.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ABB.Infrastructure.Data.Mapping
{
    public class ViewTRAkseptasiMap : IEntityTypeConfiguration<ViewTRAkseptasi>
    {
        public void Configure(EntityTypeBuilder<ViewTRAkseptasi> builder)
        {
            builder.ToTable("v_tr_akseptasi", "dbo");
            builder.HasKey(k => new { k.kd_cb, k.kd_cob, k.kd_scob, k.kd_thn, k.no_aks });
        }
    }
}