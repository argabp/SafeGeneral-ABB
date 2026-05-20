using System;

namespace ABB.Web.Modules.EntriPembayaranKas.Models
{
    public class SaveFinalPembayaranKasRequest
    {
        public string NoVoucher { get; set; }
        public DateTime? TanggalVoucher { get; set; }
    }
}