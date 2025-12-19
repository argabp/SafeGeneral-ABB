using System;

namespace ABB.Domain.Entities
{
    public class JurnalMemorial117
    {
        public string KodeCabang { get; set; }
        public string NoVoucher { get; set; } // Primary Key (bersama KodeCabang)
        public DateTime? Tanggal { get; set; }
        public string Keterangan { get; set; }
        public bool? FlagPosting { get; set; }
        
        // Audit Trail
        public DateTime? TanggalInput { get; set; }
        public string KodeUserInput { get; set; }
        public DateTime? TanggalUpdate { get; set; }
        public string KodeUserUpdate { get; set; }
    }
}