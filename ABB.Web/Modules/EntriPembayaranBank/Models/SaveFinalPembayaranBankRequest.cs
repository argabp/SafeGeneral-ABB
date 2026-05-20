using System;

namespace ABB.Web.Modules.EntriPembayaranBank.Models
{
    public class SaveFinalPembayaranBankRequest
    {
        public string NoVoucher { get; set; }
        public DateTime? TanggalVoucher { get; set; }
    }
}