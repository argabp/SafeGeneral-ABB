using System;

namespace ABB.Domain.Entities
{
    public class SourceData
    {
        public Int64 Id { get; set; }

        public string Lokasi { get; set; }

        public string KodeBisnis { get; set; }

        public string KodeProduk { get; set; }

        public string KodePas { get; set; }

        public DateTime TglTransaksi { get; set; }

        public string JenisTransaksi { get; set; }

        public string TipeTransaksi { get; set; }

        public string NoReferensi { get; set; }

        public string NoReferensi2 { get; set; }
        
        public string NoReferensi3 { get; set; }

        public string NamaReferensi { get; set; }
        
        public string NamaReferensi2 { get; set; }
        
        public string NamaReferensi3 { get; set; }

        public string Mtu { get; set; }

        public decimal Premi { get; set; }

        public decimal Disc { get; set; }

        public decimal Komisi { get; set; }

        public decimal BiaPolis { get; set; }

        public decimal BiaMaterai { get; set; }

        public decimal BiaAkusisi { get; set; }

        public decimal Netto { get; set; }

        public decimal Klaim { get; set; }

        public decimal Ppn { get; set; }

        public decimal Pph { get; set; }

        public DateTime PeriodeAwal { get; set; }

        public DateTime PeriodeAkhir { get; set; }

        public Int16 JktWaktuHari { get; set; }

        public string KodeMetode { get; set; }

        public bool FlagProses { get; set; }

        public bool FlagRelease { get; set; }

        public decimal Release { get; set; }
    }
}