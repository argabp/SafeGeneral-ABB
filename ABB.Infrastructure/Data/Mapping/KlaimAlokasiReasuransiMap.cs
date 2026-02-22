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
        }
    }
}
