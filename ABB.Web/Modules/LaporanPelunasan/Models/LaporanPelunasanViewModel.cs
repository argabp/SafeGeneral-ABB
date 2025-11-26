using System;
using System.Collections.Generic;
// using ABB.Application.VoucherBanks.Queries;

namespace ABB.Web.Modules.LaporanPelunasan.Models
{
    public class LaporanPelunasanViewModel
    {
        public string KodeCabang { get; set; }
        public string JenisAsset { get; set; }
        public string JenisAssetSD { get; set; } 
        public string BulanAwal { get; set; } 
        public string BulanAkhir { get; set; } 
        public string Tahun { get; set; } 
    }
}
