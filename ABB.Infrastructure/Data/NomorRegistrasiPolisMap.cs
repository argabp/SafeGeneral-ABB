using ABB.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ABB.Infrastructure.Data
{
    public class NomorRegistrasiPolisMap : IEntityTypeConfiguration<NomorRegistrasiPolis>
    {
        public void Configure(EntityTypeBuilder<NomorRegistrasiPolis> builder)
        {
            builder.ToTable("uw01c", "dbo");
            builder.HasKey(k => new { k.kd_cb, k.kd_cob, k.kd_scob, k.kd_thn, k.no_pol, k.no_updt });
        }
    }
}