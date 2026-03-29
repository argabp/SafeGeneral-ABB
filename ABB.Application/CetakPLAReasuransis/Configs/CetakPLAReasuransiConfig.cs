using System.Collections.Generic;
using ABB.Application.Common.Grids.Models;

namespace ABB.Application.CetakPLAReasuransis.Configs
{
    public static class CetakPLAReasuransiConfig
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
                                    grp.nm_grp_rk,
                                    rk.nm_rk,
                                    RTRIM(p.kd_cb) + '.' + RTRIM(p.kd_cob) + '.' + 
                                    RTRIM(p.kd_scob) + '.' + RTRIM(p.kd_thn) + '.' +  RTRIM(p.no_kl) + '.' + 
                                    RTRIM(p.no_mts) + '.' + RTRIM(p.no_pla) as Id,
                                    'K.' + RTRIM(p.kd_cb) + '.' + RTRIM(p.kd_scob) + '.' + 
                                    RTRIM(p.kd_thn) + '.' + RTRIM(p.no_kl) as nomor_berkas
                                 FROM v_cl08r p
                                    INNER JOIN rf01 cb ON p.kd_cb = cb.kd_cb
                                    INNER JOIN rf04 cob ON p.kd_cob = cob.kd_cob
                                    INNER JOIN rf05 scob ON p.kd_cob = scob.kd_cob 
                                                        AND p.kd_scob = scob.kd_scob
                                    INNER JOIN v_rf02 grp ON grp.kd_grp_rk = p.kd_grp_pas 
                                    INNER JOIN rf03 rk ON rk.kd_grp_rk = p.kd_grp_pas
                                                        AND rk.kd_rk = p.kd_rk_pas
                                                        AND rk.kd_cb = p.kd_cb
                            ) src
                            ",

                BaseWhere = @"
                            ",
                
                ColumnMap = new Dictionary<string, string>
                {
                    ["nomor_berkas"] = "src.nomor_berkas",
                    ["nm_cb"] = "src.nm_cb",
                    ["nm_cob"] = "src.nm_cob",
                    ["nm_scob"] = "src.nm_scob",
                    ["nm_ttg"] = "src.nm_ttg",
                    ["nm_grp_rk"] = "src.nm_grp_rk",
                    ["nm_rk"] = "src.nm_rk",
                    ["no_mts"] = "src.no_mts",
                    ["no_pla"] = "src.no_pla",
                },

                SearchableColumns = new List<string>
                {
                    "src.nomor_berkas",
                    "src.nm_cb",
                    "src.nm_cob",
                    "src.nm_scob",
                    "src.nm_ttg",
                    "src.nm_grp_rk",
                    "src.nm_rk",
                    "src.no_mts",
                    "src.no_pla",
                }
            };
        }
    }
}