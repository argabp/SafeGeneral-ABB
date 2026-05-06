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
                    FROM (  SELECT DISTINCT
                            CAST(BINARY_CHECKSUM(uw08e.kd_cb, uw08e.kd_cob, uw08e.kd_scob, uw08e.kd_thn, uw08e.no_pol, uw08e.no_updt) AS BIGINT) AS Id,
                               uw08e.kd_cb,
                               uw08e.kd_cob,
                               uw08e.kd_scob,
                               uw08e.kd_thn,
                               uw08e.kd_bln,
                               uw08e.no_pol,
                               uw08e.no_updt,
                               uw01e.nm_ttg,
                                rf01.nm_cb,
                                rf04.nm_cob,
                                rf05.nm_scob,
                            SUBSTRING(uw08e.no_pol_ttg, 1, 2) + '.' +
                            SUBSTRING(uw08e.no_pol_ttg, 3, 3) + '.' +
                            SUBSTRING(uw08e.no_pol_ttg, 6, 2) + '.' +
                            SUBSTRING(uw08e.no_pol_ttg, 8, 4) + '.' +
                            SUBSTRING(uw08e.no_pol_ttg, 12, 4) + '-' +
                            SUBSTRING(uw08e.no_pol_ttg, 16, LEN(uw08e.no_pol_ttg)) no_pol_ttg_masked
                        FROM uw01e,
                             uw08e,
                             uw04e,
                             rf01,
                             rf04,
                             rf05
                        WHERE uw01e.kd_cb = uw08e.kd_cb
                              AND uw01e.kd_cob = uw08e.kd_cob
                              AND uw01e.kd_scob = uw08e.kd_scob
                              AND uw01e.kd_thn = uw08e.kd_thn
                              AND uw01e.no_pol = uw08e.no_pol
                              AND uw01e.no_updt = uw08e.no_updt
                              AND uw01e.flag_reas = 'N'
                              AND uw08e.flag_posting = 'Y'
                              AND uw08e.flag_cancel = 'N'
                              AND uw04e.nilai_prm <> 0
                              AND uw01e.kd_cb = uw04e.kd_cb
                              AND uw01e.kd_cob = uw04e.kd_cob
                              AND uw01e.kd_scob = uw04e.kd_scob
                              AND uw01e.kd_thn = uw04e.kd_thn
                              AND uw01e.no_pol = uw04e.no_pol
                              AND uw01e.no_updt = uw04e.no_updt
                              AND uw01e.kd_cb = rf01.kd_cb
                              AND uw01e.kd_cob = rf04.kd_cob
                              AND uw01e.kd_cob = rf05.kd_cob
                              AND uw01e.kd_scob = rf05.kd_scob
                    ) as src",

                // Moving the heavy uw04e check here using EXISTS makes it very clean and fast
                BaseWhere = @"",

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