using System.Collections.Generic;
using ABB.Application.Common.Grids.Models;

namespace ABB.Application.PengajuanAkseptasi.Configs
{
    public static class AkseptasiGridRenewalConfig
    {
        public static GridConfig Create()
        {
            return new GridConfig
            {
                FromSql = @"
                            FROM (
                                SELECT 
                                    p.*
                                FROM v_uw01c_renewal p
                            ) src
                            ",
                
               BaseWhere = @"",


                ColumnMap = new Dictionary<string, string>
                {
                    ["nm_ttg"]       = "src.nm_ttg",
                    ["no_pol_ttg"]      = "src.no_pol_ttg",
                    ["tgl_mul_ptg"]      = "src.tgl_mul_ptg",
                    ["tgl_akh_ptg"]      = "src.tgl_akh_ptg"
                },
                
                SearchableColumns = new List<string>
                {
                    "src.nm_ttg",
                    "src.no_pol_ttg",
                    "src.tgl_mul_ptg",
                    "src.tgl_akh_ptg"
                }
            };
        }
    }
}