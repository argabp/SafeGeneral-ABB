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
                // We removed uw04e from the FROM clause to stop the 1.1M row multiplication
                // We also removed DISTINCT because EXISTS handles uniqueness naturally
                FromSql = @"
                    FROM (  
                        SELECT DISTINCT
                            CAST(BINARY_CHECKSUM(uw08e.kd_cb, uw08e.kd_cob, uw08e.kd_scob, uw08e.kd_thn, uw08e.no_pol, uw08e.no_updt) AS BIGINT) AS Id,
                            uw08e.kd_cb, 
                            uw08e.kd_cob,   
                            uw08e.kd_scob,   
                            uw08e.kd_thn,   
                            uw08e.no_pol,   
                            uw08e.no_updt,   
                            RTRIM(ISNULL(uw08e.kd_cb,'')) + '.' + 
                            RTRIM(ISNULL(uw08e.kd_cob,'')) + 
                            RTRIM(ISNULL(uw08e.kd_scob,'')) + '.' + 
                            RTRIM(ISNULL(uw08e.kd_thn,'')) + '.' +  
                            RTRIM(ISNULL(uw08e.no_pol,'')) + '.' + 
                            CAST(uw08e.no_updt AS VARCHAR) as nomor_akseptasi,
                            uw08e.kd_bln,   
                            uw01e.nm_ttg,     
                            uw01e.tgl_closing,   
                            uw08e.no_pol_ttg,
                            /* These columns are kept for the BaseWhere logic */
                            uw01e.flag_reas,
                            uw08e.flag_posting,
                            uw08e.flag_cancel,
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
                    ["nomor_akseptasi"] = "src.nomor_akseptasi",
                    ["tgl_closing"] = "src.tgl_closing",
                    ["no_pol_ttg_masked"] = "src.no_pol_ttg_masked",
                    ["nm_ttg"] = "src.nm_ttg"
                },

                SearchableColumns = new List<string>
                {
                    "src.nomor_akseptasi",
                    "src.nm_ttg",
                    "src.tgl_closing",
                    "src.no_pol_ttg_masked"
                }
            };
        }
    }
}