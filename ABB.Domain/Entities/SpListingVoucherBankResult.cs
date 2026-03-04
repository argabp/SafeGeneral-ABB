using System;

namespace ABB.Domain.Entities // Sesuaikan dengan namespace kamu
{
    public class SpListingVoucherBankResult
    {
        public int RowType { get; set; }
        public string NamaBank { get; set; }
        public decimal SaldoAwal { get; set; }
        public string NoVoucher { get; set; }
        public DateTime? TanggalVoucher { get; set; }
        public string KeteranganVoucher { get; set; }
        public decimal Debet { get; set; }
        public decimal Kredit { get; set; }
    }
}