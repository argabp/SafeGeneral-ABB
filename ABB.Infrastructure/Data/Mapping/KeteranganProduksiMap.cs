using ABB.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ABB.Infrastructure.Data.Mapping
{
    public class KeteranganProduksiMap : IEntityTypeConfiguration<KeteranganProduksi>
    {
        public void Configure(EntityTypeBuilder<KeteranganProduksi> builder)
        {
            // [Table("abb_kasbank")]
            builder.ToTable("abb_keterangan_produksi");

            // [Key]
           builder.HasKey(t => new 
            { 
                t.Id
            });

            // [StringLength(3)]
            builder.Property(t => t.Id)
                .HasColumnName("id_nota");

            // [Column("no_rekening")] dan [StringLength(50)]
            builder.Property(t => t.Tanggal)
                .HasColumnName("tanggal");
            
            // [Column("no_perkiraan")] dan [StringLength(50)]
            builder.Property(t => t.Keterangan)
                .HasColumnName("keterangan");

            // [Column("kasbank")] dan [StringLength(4)]
            builder.Property(t => t.NoNota)
                .HasColumnName("no_nota");
        }
    }
}