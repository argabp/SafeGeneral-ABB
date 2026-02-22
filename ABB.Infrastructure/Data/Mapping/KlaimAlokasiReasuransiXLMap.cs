using ABB.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace ABB.Infrastructure.Data.Mapping
{
    public class KlaimAlokasiReasuransiXLMap : IEntityTypeConfiguration<KlaimAlokasiReasuransiXL>
    {
        public void Configure(EntityTypeBuilder<KlaimAlokasiReasuransiXL> builder)
        {
            builder.ToTable("cl07", "dbo");
            builder.HasKey(k => new { k.kd_cb, k.kd_cob, k.kd_scob, k.kd_thn, k.no_kl, k.no_mts, k.kd_jns_sor, k.kd_kontr });
        }
    }
}
