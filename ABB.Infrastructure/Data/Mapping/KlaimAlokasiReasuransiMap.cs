using ABB.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace ABB.Infrastructure.Data.Mapping
{
    public class KlaimAlokasiReasuransiMap : IEntityTypeConfiguration<KlaimAlokasiReasuransi>
    {
        public void Configure(EntityTypeBuilder<KlaimAlokasiReasuransi> builder)
        {
            builder.ToTable("cl06", "dbo");
            builder.HasKey(k => new { k.kd_cb, k.kd_cob, k.kd_scob, k.kd_thn, k.no_kl, k.no_mts, k.kd_jns_sor, k.kd_grp_sor, k.kd_rk_sor });
            
            // Define Precision and Scale for pst_share (decimal(9,6))
            builder.Property(k => k.pst_share)
                .HasColumnType("decimal(9,6)") // Tells EF the exact SQL type
                .HasPrecision(9, 6);           // Standard EF Core way to set precision

            // Define Precision and Scale for nilai_kl (decimal(21,6))
            builder.Property(k => k.nilai_kl)
                .HasColumnType("decimal(21,6)")
                .HasPrecision(21, 6);
        }
    }
}
