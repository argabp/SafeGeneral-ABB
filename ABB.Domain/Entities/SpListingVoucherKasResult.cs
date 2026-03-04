using System;

namespace ABB.Domain.Entities // Sesuaikan
{
    public class SpListingVoucherKasResult
    {
        public int RowType { get; set; }
        public string NamaKas { get; set; }
        public decimal SaldoAwal { get; set; }
        public string NoVoucher { get; set; }
        public DateTime? TanggalVoucher { get; set; }
        public string KeteranganVoucher { get; set; }
        public decimal Debet { get; set; }
        public decimal Kredit { get; set; }
    }
}