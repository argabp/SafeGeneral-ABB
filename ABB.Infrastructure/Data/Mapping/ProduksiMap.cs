using ABB.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ABB.Infrastructure.Data.Mapping
{
    public class ProduksiMap : IEntityTypeConfiguration<Produksi>
    {
        public void Configure(EntityTypeBuilder<Produksi> builder)
        {
            builder.ToTable("produksi");

            // Memberitahu EF Core bahwa ini keyless karena kita hanya butuh membaca
            builder.HasKey(p => p.id); 
            
            // Definisikan kolom-kolomnya
            builder.Property(p => p.no_nd).HasColumnName("no_nd");
            builder.Property(p => p.nm_cust2).HasColumnName("nm_cust2");
            builder.Property(p => p.premi).HasColumnName("premi");

            // tambahan
            builder.Property(p => p.id).HasColumnName("id");
            builder.Property(p => p.no_ref).HasColumnName("no_reff");
            builder.Property(p => p.date).HasColumnName("date");
            builder.Property(p => p.type).HasColumnName("type");
            builder.Property(p => p.d_k).HasColumnName("d_k");
            builder.Property(p => p.lok).HasColumnName("lok");
            builder.Property(p => p.no_cust2).HasColumnName("no_cust2");
            builder.Property(p => p.no_brok).HasColumnName("no_brok");
            builder.Property(p => p.nm_brok).HasColumnName("nm_brok");
            builder.Property(p => p.no_endos).HasColumnName("no_endos");
            builder.Property(p => p.no_pl).HasColumnName("no_pl");
            builder.Property(p => p.curensi).HasColumnName("curensi");
            builder.Property(p => p.kurs).HasColumnName("kurs");
            builder.Property(p => p.no_kwi).HasColumnName("no_kwi");
            builder.Property(p => p.hp).HasColumnName("hp");
            builder.Property(p => p.d_per1).HasColumnName("d_per1");
            builder.Property(p => p.d_per2).HasColumnName("d_per2");
            builder.Property(p => p.no_pos).HasColumnName("no_pos");
            builder.Property(p => p.nm_pos).HasColumnName("nm_pos");
            builder.Property(p => p.rabat).HasColumnName("rabat");
            builder.Property(p => p.n_rabat).HasColumnName("n_rabat");
            builder.Property(p => p.n_bruto).HasColumnName("n_bruto");
            builder.Property(p => p.polis).HasColumnName("polis");
            builder.Property(p => p.materai).HasColumnName("materai");
            builder.Property(p => p.komisi).HasColumnName("komisi");
            builder.Property(p => p.n_komisi).HasColumnName("n_komisi");
            builder.Property(p => p.h_fee).HasColumnName("h_fee");
            builder.Property(p => p.n_hfee).HasColumnName("n_hfee");
            builder.Property(p => p.lain).HasColumnName("lain");
            builder.Property(p => p.klaim).HasColumnName("klaim");
            builder.Property(p => p.netto).HasColumnName("netto");
            builder.Property(p => p.tgl_byr).HasColumnName("tgl_byr");
            builder.Property(p => p.jumlah).HasColumnName("jumlah");
            builder.Property(p => p.date_input).HasColumnName("date_input");
            builder.Property(p => p.date_edit).HasColumnName("date_edit");
            builder.Property(p => p.created_by).HasColumnName("created_by");
            builder.Property(p => p.edited_by).HasColumnName("edited_by");

            builder.Property(p => p.jn_ass).HasColumnName("jn_ass");
            builder.Property(p => p.qq).HasColumnName("qq");
            builder.Property(p => p.tgl_jth_tempo).HasColumnName("tgl_jth_tempo");
            builder.Property(p => p.reg).HasColumnName("reg");
            builder.Property(p => p.catat).HasColumnName("catat");
            builder.Property(p => p.kd_ass2).HasColumnName("kd_ass2");
            builder.Property(p => p.kd_tutup).HasColumnName("kd_tutup");
            builder.Property(p => p.kd_ass3).HasColumnName("kd_ass3");
            builder.Property(p => p.debet).HasColumnName("debet");
            builder.Property(p => p.nm_pol).HasColumnName("nm_pol");
            builder.Property(p => p.saldo).HasColumnName("saldo");
            builder.Property(p => p.kredit).HasColumnName("kredit");
            builder.Property(p => p.no_bukti).HasColumnName("no_bukti");
            builder.Property(p => p.post_tr).HasColumnName("post_tr");
            builder.Property(p => p.jn_ass2).HasColumnName("jn_ass2");
            builder.Property(p => p.no_nd2).HasColumnName("no_nd2");

        }
    }
}