using System;

namespace ABB.Domain.Entities
{
    public class JurnalMemorial104
    {
        public string NoVoucher { get; set; }
        public string KodeCabang { get; set; }
        public DateTime? Tanggal { get; set; }
        public string Keterangan { get; set; }
        public DateTime? TanggalInput { get; set; }
        public string KodeUserInput { get; set; }
        public DateTime? TanggalUpdate { get; set; }
        public string KodeUserUpdate { get; set; }
        public bool? FlagGL { get; set; }
    }
}