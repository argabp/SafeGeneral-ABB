using ABB.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ABB.Infrastructure.Data.Mapping
{
    public class NotaKomisiTambahanMap : IEntityTypeConfiguration<NotaKomisiTambahan>
    {
        public void Configure(EntityTypeBuilder<NotaKomisiTambahan> builder)
        {
            builder.ToTable("fn05", "dbo");
            builder.HasKey(k => new
            {
                k.jns_sb_nt, k.kd_cb, k.jns_tr, k.jns_nt_msk,
                k.kd_thn, k.kd_bln, k.no_nt_msk, k.jns_nt_kel,
                k.no_nt_kel
            });
        }
    }
}