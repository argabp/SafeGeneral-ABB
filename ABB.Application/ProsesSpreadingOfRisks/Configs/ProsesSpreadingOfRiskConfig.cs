using System.Collections.Generic;
using ABB.Application.Common.Grids.Models;

namespace ABB.Application.ProsesSpreadingOfRisks.Configs
{
    public static class ProsesSpreadingOfRiskConfig
    {
        public static GridConfig Create()
        {
            return new GridConfig
            {
                FromSql = @"
                    FROM (  
                        SELECT DISTINCT
                            CAST(BINARY_CHECKSUM(uw08e.kd_cb, uw08e.kd_cob, uw08e.kd_scob, uw08e.kd_thn, uw08e.no_pol, uw08e.no_updt) AS BIGINT) AS Id,
                            uw08e.*, 
                            uw01e.*,
                            cb.nm_cb,
                            cob.nm_cob,
                            scob.nm_scob,
                            SUBSTRING(uw08e.no_pol_ttg, 1, 2) + '.' +
                            SUBSTRING(uw08e.no_pol_ttg, 3, 3) + '.' +
                            SUBSTRING(uw08e.no_pol_ttg, 6, 2) + '.' +
                            SUBSTRING(uw08e.no_pol_ttg, 8, 4) + '.' +
                            SUBSTRING(uw08e.no_pol_ttg, 12, 4) + '-' +
                            SUBSTRING(uw08e.no_pol_ttg, 16, LEN(uw08e.no_pol_ttg)) no_pol_ttg_masked
                        FROM uw08e
                        JOIN uw01e ON uw01e.kd_cb = uw08e.kd_cb AND uw01e.kd_cob = uw08e.kd_cob 
                                  AND uw01e.kd_scob = uw08e.kd_scob AND uw01e.kd_thn = uw08e.kd_thn 
                                  AND uw01e.no_pol = uw08e.no_pol AND uw01e.no_updt = uw08e.no_updt
                        INNER JOIN rf01 cb ON uw08e.kd_cb = cb.kd_cb
                        INNER JOIN rf04 cob ON uw08e.kd_cob = cob.kd_cob
                        INNER JOIN rf05 scob ON uw08e.kd_cob = scob.kd_cob 
                                            AND uw08e.kd_scob = scob.kd_scob
                    ) as src",

                // Moving the heavy uw04e check here using EXISTS makes it very clean and fast
                BaseWhere = @"
                    src.flag_reas = 'N' 
                    AND src.flag_posting = 'Y' 
                    AND src.flag_cancel = 'N'
                    AND EXISTS (
                        SELECT 1 FROM uw04e 
                        WHERE uw04e.kd_cb = src.kd_cb 
                          AND uw04e.kd_cob = src.kd_cob 
                          AND uw04e.kd_scob = src.kd_scob 
                          AND uw04e.kd_thn = src.kd_thn 
                          AND uw04e.no_pol = src.no_pol 
                          AND uw04e.no_updt = src.no_updt 
                          AND uw04e.nilai_prm <> 0
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