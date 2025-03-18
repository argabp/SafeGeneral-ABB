using ABB.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ABB.Infrastructure.Data.Mapping
{
    public class AkseptasiMap : IEntityTypeConfiguration<Akseptasi>
    {
        public void Configure(EntityTypeBuilder<Akseptasi> builder)
        {
            builder.ToTable("uw01a", "dbo");
            builder.HasKey(k => new { k.kd_cb, k.kd_cob, k.kd_scob, k.kd_thn, k.no_aks, k.no_updt });

            builder.Property(p => p.thn_uw).HasPrecision(4, 0);
            builder.Property(p => p.pst_share_bgu).HasPrecision(9, 6);
            builder.Property(p => p.faktor_prd).HasPrecision(11, 6);
        }
    }
}