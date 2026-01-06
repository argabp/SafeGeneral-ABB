using ABB.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ABB.Infrastructure.Data.Mapping
{
    public class Jurnal62Map : IEntityTypeConfiguration<Jurnal62>
    {
        public void Configure(EntityTypeBuilder<Jurnal62> builder)
        {
            builder.ToTable("abb_jurnal62");

            builder.HasKey(t => t.Id);

            builder.Property(t => t.Id)
                .HasColumnName("id"); 

            builder.Property(t => t.GlLok)
                .HasColumnName("gl_lok");   

            builder.Property(t => t.GlTran)
                .HasColumnName("gl_tran");   

            builder.Property(t => t.GlBukti)
                .HasColumnName("gl_bukti"); 

            builder.Property(t => t.GlTanggal)
                .HasColumnName("gl_tgl");

            builder.Property(t => t.GlNota)
                .HasColumnName("gl_nota");

            builder.Property(t => t.GlMtu)
                .HasColumnName("gl_mtu");

            builder.Property(t => t.GlKet)
                .HasColumnName("gl_ket");

            builder.Property(t => t.GlUrut)
                .HasColumnName("gl_urut");

            builder.Property(t => t.GlDk)
                .HasColumnName("gl_dk"); 

            builder.Property(t => t.GlNilaiOrg)
                .HasColumnName("gl_nilai_org");  

            builder.Property(t => t.GlNilaiIdr)
                .HasColumnName("gl_nilai_idr"); 
                
            builder.Property(t => t.GlAkun)
                .HasColumnName("gl_akun"); 

            builder.Property(t => t.FlagClosed)
                .HasColumnName("flag_closed"); 

            builder.Property(t => t.TglInput)
                .HasColumnName("tgl_input"); 

            builder.Property(t => t.KodeUserInput)
                .HasColumnName("kd_user_input"); 

            builder.Property(t => t.TglUpdate)
                .HasColumnName("tgl_update"); 

            builder.Property(t => t.KodeUserUpdate)
                .HasColumnName("kd_user_update"); 
        }
    }
}