using System.Collections.Generic;
using ABB.Application.Common.Grids.Models;

namespace ABB.Application.CetakNotaPremiFakultatifMasuks.Configs
{
    public static class CetakNotaPremiFakultatifMasukConfig
    {
        public static GridConfig Create()
        {
            return new GridConfig
            {
                FromSql = @"
                            FROM (
                                SELECT 
                                    p.*,
                                    cb.nm_cb,
                                    cob.nm_cob,
                                    scob.nm_scob,
                                    RTRIM(p.kd_cb) + 
                                    RTRIM(p.kd_cob) + 
                                    RTRIM(p.kd_scob) + 
                                    RTRIM(p.kd_thn) + 
                                    RTRIM(p.no_pol) + 
                                    CAST(p.no_updt AS VARCHAR) AS Id,
                                    RTRIM(p.kd_cb) + '.' + RTRIM(p.kd_cob) + RTRIM(p.kd_scob) + '.' + 
                                    RTRIM(p.kd_thn) + '.' + RTRIM(p.no_pol) AS nomor_akseptasi,
                                    SUBSTRING(p.no_pol_ttg, 1, 2) + '.' +
                                    SUBSTRING(p.no_pol_ttg, 3, 3) + '.' +
                                    SUBSTRING(p.no_pol_ttg, 6, 2) + '.' +
                                    SUBSTRING(p.no_pol_ttg, 8, 4) + '.' +
                                    SUBSTRING(p.no_pol_ttg, 12, 4) + '-' +
                                    SUBSTRING(p.no_pol_ttg, 16, LEN(p.no_pol_ttg)) no_pol_ttg_masked
                                 FROM uw01e p
                                    INNER JOIN rf01 cb ON p.kd_cb = cb.kd_cb
                                    INNER JOIN rf04 cob ON p.kd_cob = cob.kd_cob
                                    INNER JOIN rf05 scob ON p.kd_cob = scob.kd_cob 
                                                        AND p.kd_scob = scob.kd_scob
                            ) src
                            ",
                
               BaseWhere = "(src.st_pas = 'C')",


                ColumnMap = new Dictionary<string, string>
                {
                    ["nm_cb"]       = "src.nm_cb",
                    ["nm_cob"]      = "src.nm_cob",
                    ["nm_scob"]     = "src.nm_scob",
                    ["nm_mtu"] = "src.nm_mtu",
                    ["nomor_akseptasi"] = "src.nomor_akseptasi",
                    ["no_pol_ttg_masked"] = "src.no_pol_ttg_masked",
                    ["no_updt"] = "src.no_updt",
                    ["tgl_closing"] = "src.tgl_closing",
                    ["nm_ttg"] = "src.nm_ttg",
                    ["user_input"] = "src.user_input"
                },
                
                SearchableColumns = new List<string>
                {
                    "src.nm_cb",
                    "src.nm_cob",
                    "src.nm_scob",
                    "src.nm_mtu",
                    "src.nomor_akseptasi",
                    "src.no_pol_ttg_masked",
                    "src.no_updt",
                    "src.tgl_closing",
                    "src.nm_ttg",
                    "src.user_input",
                }
            };
        }
    }
}