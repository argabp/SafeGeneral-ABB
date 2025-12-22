using System;

namespace ABB.Domain.Entities
{
    public class JurnalMemorial117Detail
    {
        public string NoVoucher { get; set; } // Composite PK
        public int No { get; set; }           // Composite PK
        
        public string KodeAkun { get; set; }
        public string FlagPosting { get; set; }
        // public string NoNota { get; set; }
        public string Keterangan { get; set; }
        public string KodeMataUang { get; set; }
        public decimal? NilaiDebet { get; set; }
        public decimal? NilaiDebetRp { get; set; }
        public decimal? NilaiKredit { get; set; }
        public decimal? NilaiKreditRp { get; set; }
        
        // Audit Trail
        public DateTime? TanggalBayar { get; set; }
        public string UserBayar { get; set; }
        public DateTime? TanggalUserInput { get; set; }
        public string KodeUserInput { get; set; }
        public DateTime? TanggalUserUpdate { get; set; }
        public string KodeUserUpdate { get; set; }
    }
}