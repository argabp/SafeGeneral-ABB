using System;

namespace ABB.Application.CancelCSM.Queries
{
    public class ViewSourceDataCancelDto
    {
        public DateTime PeriodeProses { get; set; }
        
        public Int64 Id { get; set; }

        public string JenisTransaksi { get; set; }

        public string TipeTransaksi { get; set; }

        public string NoReferensi { get; set; }

        public string NoReferensi2 { get; set; }

        public string NamaReferensi { get; set; }

        public DateTime PeriodeAwal { get; set; }

        public DateTime PeriodeAkhir { get; set; }

        public Int16 JktWaktuHari { get; set; }

        public string Mtu { get; set; }

        public decimal Premi { get; set; }

        public decimal Disc { get; set; }

        public decimal Komisi { get; set; }

        public decimal BiaAkusisi { get; set; }

        public decimal Netto { get; set; }

        public bool FlagProses { get; set; }
    }
}