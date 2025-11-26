using System;

namespace ABB.Domain.Entities
{
    public class EntriPembayaranKas
    {
        // Bagian dari Composite Primary Key
        public string NoVoucher { get; set; }
        // Bagian dari Composite Primary Key
        public int No { get; set; }

        public string FlagPosting { get; set; }
        public string KodeAkun { get; set; }
        public string FlagPembayaran { get; set; }
        public string NoNota4 { get; set; }
        public string KodeMataUang { get; set; }
        public decimal? TotalBayar { get; set; }
        public string UserBayar { get; set; }
        public string DebetKredit { get; set; }
        public decimal? TotalDlmRupiah { get; set; }
         public int? Kurs { get; set; }
    }
}