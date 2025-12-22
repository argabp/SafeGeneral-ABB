using System;

namespace ABB.Domain.Entities
{
    public class EntriPenyelesaianPiutangTemp
    {
        // Bagian dari Composite Primary Key
        public string NoBukti { get; set; }
        // Bagian dari Composite Primary Key
        public int No { get; set; }

        public string KodeAkun { get; set; }
        public string FlagPembayaran { get; set; }
        public string NoNota { get; set; }
        public string KodeMataUang { get; set; }
        public decimal? TotalBayarOrg { get; set; }
        public decimal? TotalBayarRp { get; set; }
        public string UserBayar { get; set; }
        public string DebetKredit { get; set; }

          // tambahan sebagai audit
         public string KodeUserInput { get; set; }
        public string KodeUserUpdate { get; set; }

        public DateTime? TanggalInput { get; set; }
        public DateTime? TanggalUpdate { get; set; }
    }
}