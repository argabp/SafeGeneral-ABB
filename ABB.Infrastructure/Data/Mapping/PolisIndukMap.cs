using ABB.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ABB.Infrastructure.Data.Mapping
{
    public class PolisIndukMap : IEntityTypeConfiguration<PolisInduk>
    {
        public void Configure(EntityTypeBuilder<PolisInduk> builder)
        {
            builder.ToTable("uw10", "dbo");
            builder.HasKey(k => k.no_pol_induk);

            builder.Property(p => p.thn_uw).HasPrecision(4, 0);
            builder.Property(p => p.pst_share_bgu).HasPrecision(9, 6);
            builder.Property(p => p.faktor_prd).HasPrecision(11, 6);
            builder.Property(p => p.pst_prm).HasPrecision(9, 6);
            builder.Property(p => p.pst_dis).HasPrecision(9, 6);
            builder.Property(p => p.pst_kms).HasPrecision(9, 6);
            builder.Property(p => p.pst_insentif).HasPrecision(9, 6);
            builder.Property(p => p.nilai_min_prm).HasPrecision(21, 6);
            builder.Property(p => p.nilai_deposit).HasPrecision(21, 6);
            builder.Property(p => p.nilai_tsi).HasPrecision(21, 6);
        }
    }
}