using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ABB.Domain.Entities
{
    [Table("abb_template_lapkeu")] // Mapping ke nama tabel di DB
    public class TemplateLapKeu
    {
        [Key]
        [Column("id")]
        public long Id { get; set; } // bigint di DB = long di C#

        [Column("tipe_laporan")]
        public string TipeLaporan { get; set; } // Contoh: 'NERACA', 'LABARUGI'

        [Column("tipe_baris")]
        public string TipeBaris { get; set; } // Contoh: 'HEADER', 'DETAIL', 'TOTAL'

        [Column("deskripsi")]
        public string Deskripsi { get; set; } // Contoh: 'ASET LANCAR'

        [Column("rumus")]
        public string Rumus { get; set; } // Contoh: '1101,1102' atau 'SUM(ROW_1)'

        [Column("level")]
        public string Level { get; set; } // Contoh: '1', '2' (untuk indentasi)
    }
}