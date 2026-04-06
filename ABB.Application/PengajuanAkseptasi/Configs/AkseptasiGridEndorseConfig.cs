using System.Collections.Generic;
using ABB.Application.Common.Grids.Models;

namespace ABB.Application.PengajuanAkseptasi.Configs
{
    public static class AkseptasiGridEndorseConfig
    {
        public static GridConfig Create()
        {
            return new GridConfig
            {
                FromSql = @"
                            FROM (
                                SELECT 
                                    p.nm_ttg,
                                    p.tgl_mul_ptg,
                                    p.tgl_akh_ptg,
                                    p.no_pol_ttg,
                                    SUBSTRING(p.no_pol_ttg, 1, 2) + '.' +
                                    SUBSTRING(p.no_pol_ttg, 3, 3) + '.' +
                                    SUBSTRING(p.no_pol_ttg, 6, 2) + '.' +
                                    SUBSTRING(p.no_pol_ttg, 8, 4) + '.' +
                                    SUBSTRING(p.no_pol_ttg, 12, 4) + '-' +
                                    SUBSTRING(p.no_pol_ttg, 16, LEN(no_pol_ttg)) no_pol_ttg_masked
                                FROM v_uw01c_endorse p
                            ) src
                            ",
                
               BaseWhere = @"",


                ColumnMap = new Dictionary<string, string>
                {
                    ["nm_ttg"]       = "src.nm_ttg",
                    ["no_pol_ttg_masked"]      = "src.no_pol_ttg_masked",
                    ["tgl_mul_ptg"]      = "src.tgl_mul_ptg",
                    ["tgl_akh_ptg"]      = "src.tgl_akh_ptg"
                },
                
                SearchableColumns = new List<string>
                {
                    "src.nm_ttg",
                    "src.no_pol_ttg_masked",
                    "src.tgl_mul_ptg",
                    "src.tgl_akh_ptg"
                }
            };
        }
    }
}