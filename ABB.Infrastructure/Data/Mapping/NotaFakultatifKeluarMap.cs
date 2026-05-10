using ABB.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ABB.Infrastructure.Data.Mapping
{
    public class NotaFakultatifKeluarMap : IEntityTypeConfiguration<NotaFakultatifKeluar>
    {
        public void Configure(EntityTypeBuilder<NotaFakultatifKeluar> builder)
        {
            builder.ToTable("ri04e", "dbo");
            builder.HasKey(k => new
            {
                k.kd_cb, k.jns_tr, k.jns_nt_msk, k.kd_thn, k.kd_bln,
                k.no_nt_msk, k.jns_nt_kel, k.no_nt_kel
            });
        }
    }
}