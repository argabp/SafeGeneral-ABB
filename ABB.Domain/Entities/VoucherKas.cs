using System;

namespace ABB.Domain.Entities
{
    public class VoucherKas
    {
        public string KodeCabang { get; set; }
        public string JenisVoucher { get; set; }
        public string DebetKredit { get; set; }
        public string NoVoucher { get; set; }
        public string KodeAkun { get; set; }
        public string DibayarKepada { get; set; }
        public DateTime? TanggalVoucher { get; set; }
        public string KodeMataUang { get; set; }
        public decimal? TotalVoucher { get; set; }
        public decimal? TotalDalamRupiah { get; set; }
        public string KeteranganVoucher { get; set; }
        public bool? FlagPosting { get; set; }
        public DateTime? TanggalInput { get; set; }
        public DateTime? TanggalUpdate { get; set; }
        public string KodeUserInput { get; set; }
        public string KodeUserUpdate { get; set; }
        public string JenisPembayaran { get; set; }
    }
}