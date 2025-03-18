using ABB.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ABB.Infrastructure.Data.Mapping
{
    public class PesertaStatusDokumenKrediturMap : IEntityTypeConfiguration<PesertaStatusDokumenKreditur>
    {
        public void Configure(EntityTypeBuilder<PesertaStatusDokumenKreditur> builder)
        {
            builder.ToTable("TR_PesertaStatus2Dokumen", "dbo");
            builder.HasKey(k => new { k.kd_cb, k.kd_product, k.kd_thn, k.kd_rk, k.no_sppa, k.no_updt, k.no_urut, k.no_dokumen });
        }
    }
}