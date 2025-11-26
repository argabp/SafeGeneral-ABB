using ABB.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ABB.Infrastructure.Data.Mapping
{
    public class KasBankMap : IEntityTypeConfiguration<KasBank>
    {
        public void Configure(EntityTypeBuilder<KasBank> builder)
        {
            // [Table("abb_kasbank")]
            builder.ToTable("abb_kasbank");

            // [Key]
            builder.HasKey(t => t.Kode);

            // [StringLength(3)]
            builder.Property(t => t.Kode)
                .HasMaxLength(3)
                .IsRequired(); // Primary Key biasanya required

            // [Required] dan [StringLength(75)]
            builder.Property(t => t.Keterangan)
                .HasMaxLength(75)
                .IsRequired();

            // [Column("no_rekening")] dan [StringLength(50)]
            builder.Property(t => t.NoRekening)
                .HasColumnName("no_rekening")
                .HasMaxLength(50);
            
            // [Column("no_perkiraan")] dan [StringLength(50)]
            builder.Property(t => t.NoPerkiraan)
                .HasColumnName("no_perkiraan")
                .HasMaxLength(50);

            // [Column("kasbank")] dan [StringLength(4)]
            builder.Property(t => t.TipeKasBank)
                .HasColumnName("kasbank")
                .HasMaxLength(4);

            builder.Property(t => t.Saldo).HasColumnName("saldo").HasPrecision(18,2);
        }
    }
}