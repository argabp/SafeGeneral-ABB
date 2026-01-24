using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ABB.Domain.Entities
{
    [Table("abb_rekapjurnal62")] // Mapping ke nama tabel
    public class RekapJurnal
    {
        // Sesuaikan Primary Key jika ada, atau gunakan [Key] pada kolom unik
        // Jika tidak ada PK, EF Core butuh konfigurasi khusus (HasNoKey), 
        // tapi anggap saja 'id' atau komposit key ada.
        
        [Column("gl_akun")]
        public string gl_akun { get; set; }

        [Column("gl_dk")]
        public string gl_dk { get; set; } // "D" atau "K"

        [Column("gl_nilai_idr")]
        // UBAH DARI DECIMAL KE DOUBLE
        public double gl_nilai_idr { get; set; }

        [Column("thn")]
        public int thn { get; set; }

        [Column("bln")]
        public int bln { get; set; }

        // Tambahkan properti lain jika perlu mapping ID/Key
        // public long Id { get; set; } 
    }
}