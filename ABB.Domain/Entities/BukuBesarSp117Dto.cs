using System;

namespace ABB.Domain.Entities
{
    // Class ini hanya untuk menampung hasil SP, bukan tabel asli
    public class BukuBesarSp117Dto
    {
        public string KodeAkun { get; set; }
        public string NamaAkun { get; set; }
        public decimal? SaldoAwal { get; set; }
        public DateTime? Tanggal { get; set; }
        public string NoBukti { get; set; }
        public string Keterangan { get; set; }
        public decimal? Debet { get; set; }
        public decimal? Kredit { get; set; }
        public int RowType { get; set; }
    }
}