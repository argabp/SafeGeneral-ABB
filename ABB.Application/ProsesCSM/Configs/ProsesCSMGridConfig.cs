using System.Collections.Generic;
using ABB.Application.Common.Grids.Models;

namespace ABB.Application.ProsesCSM.Configs
{
    public static class ProsesCSMGridConfig
    {
        public static GridConfig Create()
        {
            return new GridConfig
            {
                FromSql = @"
                            FROM (
                                SELECT * FROM v_MS_SourceData
                            ) src
                            ",
                
                BaseWhere = @"
                            (src.KodeMetode = @KodeMetode)
                            ",

                ColumnMap = new Dictionary<string, string>
                {
                    ["Id"]              = "src.Id",
                    ["PeriodeProses"]   = "src.PeriodeProses",
                    ["JenisTransaksi"]  = "src.JenisTransaksi",
                    ["TipeTransaksi"]   = "src.TipeTransaksi",
                    ["NoReferensi"]     = "src.NoReferensi",
                    ["NoReferensi2"]    = "src.NoReferensi2",
                    ["NamaReferensi"]   = "src.NamaReferensi",
                    ["PeriodeAwal"]     = "src.PeriodeAwal",
                    ["PeriodeAkhir"]    = "src.PeriodeAkhir",
                    ["JktWaktuHari"]    = "src.JktWaktuHari",
                    ["Mtu"]             = "src.Mtu",
                    ["Premi"]           = "src.Premi",
                    ["Disc"]            = "src.Disc",
                    ["Komisi"]          = "src.Komisi",
                    ["BiaAkusisi"]      = "src.BiaAkusisi",
                    ["Netto"]           = "src.Netto",
                    ["FlagProses"]      = "src.FlagProses"
                },
                
                SearchableColumns = new List<string>
                {
                    "src.Id",
                    "CAST(src.PeriodeProses AS VARCHAR)",
                    "src.JenisTransaksi",
                    "src.TipeTransaksi",
                    "src.NoReferensi",
                    "src.NoReferensi2",
                    "src.NamaReferensi",
                    "CAST(src.PeriodeAwal AS VARCHAR)",
                    "CAST(src.PeriodeAkhir AS VARCHAR)",
                    "src.JktWaktuHari",
                    "src.Mtu",
                    "CAST(src.Premi AS VARCHAR)",
                    "CAST(src.Disc AS VARCHAR)",
                    "CAST(src.Komisi AS VARCHAR)",
                    "CAST(src.BiaAkusisi AS VARCHAR)",
                    "CAST(src.Netto AS VARCHAR)",
                    "src.FlagProses"
                }
            };
        }
    }
}