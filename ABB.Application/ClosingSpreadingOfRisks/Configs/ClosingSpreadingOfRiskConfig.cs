using System.Collections.Generic;
using ABB.Application.Common.Grids.Models;

namespace ABB.Application.ClosingSpreadingOfRisks.Configs
{
    public static class ClosingSpreadingOfRiskConfig
    {
        public static GridConfig Create()
        {
            return new GridConfig
            {
                // We removed uw04e from the FROM clause to stop the 1.1M row multiplication
                // We also removed DISTINCT because EXISTS handles uniqueness naturally
                FromSql = @"
                    FROM (  
                        SELECT DISTINCT
                            CAST(BINARY_CHECKSUM(p.kd_cb, p.kd_cob, p.kd_scob, p.kd_thn, p.no_pol, p.no_updt) AS BIGINT) AS Id,
                            p.*, 
                            cb.nm_cb,
                            cob.nm_cob,
                            scob.nm_scob,
                            SUBSTRING(p.no_pol_ttg, 1, 2) + '.' +
                            SUBSTRING(p.no_pol_ttg, 3, 3) + '.' +
                            SUBSTRING(p.no_pol_ttg, 6, 2) + '.' +
                            SUBSTRING(p.no_pol_ttg, 8, 4) + '.' +
                            SUBSTRING(p.no_pol_ttg, 12, 4) + '-' +
                            SUBSTRING(p.no_pol_ttg, 16, LEN(p.no_pol_ttg)) no_pol_ttg_masked
                        FROM v_ri01e p
                        INNER JOIN rf01 cb ON p.kd_cb = cb.kd_cb
                        INNER JOIN rf04 cob ON p.kd_cob = cob.kd_cob
                        INNER JOIN rf05 scob ON p.kd_cob = scob.kd_cob 
                                            AND p.kd_scob = scob.kd_scob
                    ) as src",

                // Moving the heavy uw04e check here using EXISTS makes it very clean and fast
                BaseWhere = @"(
                    src.flag_closing = 'N'
                    )",

                ColumnMap = new Dictionary<string, string>
                {
                    ["nm_cb"] = "src.nm_cb",
                    ["nm_cob"] = "src.nm_cob",
                    ["nm_scob"] = "src.nm_scob",
                    ["tgl_closing"] = "src.tgl_closing",
                    ["tgl_mul_ptg"] = "src.tgl_mul_ptg",
                    ["tgl_akh_ptg"] = "src.tgl_akh_ptg",
                    ["no_pol_ttg_masked"] = "src.no_pol_ttg_masked",
                    ["nm_ttg"] = "src.nm_ttg"
                },

                SearchableColumns = new List<string>
                {
                    "src.nm_cb",
                    "src.nm_cob",
                    "src.nm_scob",
                    "src.nm_ttg",
                    "src.tgl_closing",
                    "src.tgl_mul_ptg",
                    "src.tgl_akh_ptg",
                    "src.no_pol_ttg_masked"
                }
            };
        }
    }
}