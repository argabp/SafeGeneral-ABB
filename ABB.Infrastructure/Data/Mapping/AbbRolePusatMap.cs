using ABB.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ABB.Infrastructure.Data.Mapping
{
    public class AbbRolePusatMap : IEntityTypeConfiguration<AbbRolePusat>
    {
        public void Configure(EntityTypeBuilder<AbbRolePusat> builder)
        {
            // Nama tabel di database (dan skemanya, defaultnya biasanya "dbo")
            builder.ToTable("abb_role_pusat", "dbo");

            // Set Primary Key
            builder.HasKey(k => k.role_code);

            // Konfigurasi kolom
            builder.Property(p => p.role_code)
                   .IsRequired(); // role_code wajib diisi (Not Null)

            builder.Property(p => p.role_nama)
                   .HasMaxLength(255) // Varchar(255) sesuai di database
                   .HasColumnType("varchar(255)"); // Opsional, untuk mempertegas tipe datanya
        }
    }
}