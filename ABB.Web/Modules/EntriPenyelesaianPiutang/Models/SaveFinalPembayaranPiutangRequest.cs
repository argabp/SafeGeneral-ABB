using System;

namespace ABB.Web.Modules.EntriPenyelesaianPiutang.Models
{
    public class SaveFinalPembayaranPiutangRequest
    {
        public string noBukti { get; set; }
        public DateTime? Tanggal { get; set; }
    }
}