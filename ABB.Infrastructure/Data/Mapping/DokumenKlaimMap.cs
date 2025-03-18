using ABB.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ABB.Infrastructure.Data.Mapping
{
    public class DokumenKlaimMap : IEntityTypeConfiguration<DokumenKlaim>
    {
        public void Configure(EntityTypeBuilder<DokumenKlaim> builder)
        {
            builder.ToTable("dp20", "dbo");
            builder.HasKey(k => new { k.kd_cob, k.kd_dok });
        }
    }
}