using ABB.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ABB.Infrastructure.Data.Mapping
{
    public class LimitKlaimMap : IEntityTypeConfiguration<LimitKlaim>
    {
        public void Configure(EntityTypeBuilder<LimitKlaim> builder)
        {
            builder.ToTable("MS_LimitKlaim", "dbo");
            builder.HasKey(k => new { k.kd_cb, k.kd_cob, k.kd_scob, k.thn });
        }
    }
}