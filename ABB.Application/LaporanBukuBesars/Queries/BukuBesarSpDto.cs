using System;
namespace ABB.Application.LaporanBukuBesars.Queries // (Atau namespace yang kamu pakai)
{
    public class BukuBesarSpDto
    {
        public string KodeAkun { get; set; }
        public string NamaAkun { get; set; }
        public decimal? SaldoAwal { get; set; }
        
        // DateTime butuh "using System;"
        public DateTime? Tanggal { get; set; } 
        
        public string NoBukti { get; set; }
        public string Keterangan { get; set; }
        public decimal? Debet { get; set; }
        public decimal? Kredit { get; set; }
        public int RowType { get; set; }
    }
}