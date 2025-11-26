using System;

namespace ABB.Domain.Entities
{
    public class VoucherBank
    {
        public string KodeCabang { get; set; }
        public string JenisVoucher { get; set; }
        public string DebetKredit { get; set; }
        public string NoVoucher { get; set; }
        public string KodeAkun { get; set; }
        // DIUBAH: 'DibayarKepada' menjadi 'DiterimaDari' agar sesuai dengan kolom 'diterima_dari'
        public string DiterimaDari { get; set; }
        public DateTime? TanggalVoucher { get; set; }
        public string KodeMataUang { get; set; }
        public decimal? TotalVoucher { get; set; }
        public decimal? TotalDalamRupiah { get; set; }
        public string KeteranganVoucher { get; set; }
        public bool? FlagPosting { get; set; }
        public bool? FlagFinal { get; set; }
        public DateTime? TanggalInput { get; set; }
        public DateTime? TanggalUpdate { get; set; }
        public string KodeUserInput { get; set; }
        public string KodeUserUpdate { get; set; }
        // DITAMBAHKAN: Dua properti ini tidak ada sebelumnya
        public string KodeBank { get; set; }
        public string NoBank { get; set; }
         public string JenisPembayaran { get; set; }
    }
}