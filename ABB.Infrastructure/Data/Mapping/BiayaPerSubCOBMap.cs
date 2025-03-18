using ABB.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ABB.Infrastructure.Data.Mapping
{
    public class BiayaPerSubCOBMap : IEntityTypeConfiguration<BiayaPerSubCOB>
    {
        public void Configure(EntityTypeBuilder<BiayaPerSubCOB> builder)
        {
            builder.ToTable("dp03", "dbo");
            builder.HasKey(k => new { k.kd_cob, k.kd_scob, k.kd_mtu });

            builder.Property(p => p.nilai_bia_adm).HasPrecision(21, 6);
            builder.Property(p => p.nilai_bia_pol).HasPrecision(21, 6);
            builder.Property(p => p.nilai_maks_plafond).HasPrecision(21, 6);
            builder.Property(p => p.nilai_min_form).HasPrecision(21, 6);
            builder.Property(p => p.nilai_min_prm).HasPrecision(21, 6);
        }
    }
}