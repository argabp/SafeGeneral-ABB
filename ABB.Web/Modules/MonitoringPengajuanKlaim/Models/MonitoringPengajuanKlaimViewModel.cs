using System;

namespace ABB.Web.Modules.MonitoringPengajuanKlaim.Models
{
    public class MonitoringPengajuanKlaimViewModel
    {
        public string KodeCabang { get; set; }

        public DateTime TanggalAwal { get; set; }

        public DateTime TanggalAkhir { get; set; }
    }
}