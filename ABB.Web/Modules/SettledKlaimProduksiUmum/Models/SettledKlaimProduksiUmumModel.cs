using System;

namespace ABB.Web.Modules.SettledKlaimProduksiUmum.Models
{
    public class SettledKlaimProduksiUmumModel
    {
        public string KodeCabang { get; set; }

        public DateTime TanggalAwal { get; set; }

        public DateTime TanggalAkhir { get; set; }
    }
}