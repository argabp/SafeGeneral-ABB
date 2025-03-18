using ABB.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ABB.Infrastructure.Data.Mapping
{
    public class NotaDinasDokumentMap : IEntityTypeConfiguration<NotaDinasDokumen>
    {
        public void Configure(EntityTypeBuilder<NotaDinasDokumen> builder)
        {
            builder.ToTable("TR_NotaDinasDokumen", "dbo");
            builder.HasKey(k => k.id_nds);
        }
    }
}