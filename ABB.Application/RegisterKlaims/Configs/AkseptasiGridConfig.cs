using System.Collections.Generic;
using ABB.Application.Common.Grids.Models;

namespace ABB.Application.RegisterKlaims.Configs
{
    public static class AkseptasiGridConfig
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
                                    scob.nm_scob
                                FROM v_uw04e p
                                INNER JOIN rf01 cb ON p.kd_cb = cb.kd_cb
                                INNER JOIN rf04 cob ON p.kd_cob = cob.kd_cob
                                INNER JOIN rf05 scob ON p.kd_cob = scob.kd_cob 
                                                    AND p.kd_scob = scob.kd_scob
                            ) src
                            ",
                
               BaseWhere = @"
                            src.kd_cb = @KodeCabang
                            AND src.kd_cob = @kd_cob
                            AND src.kd_scob = @kd_scob
                            ",


                ColumnMap = new Dictionary<string, string>
                {
                    ["nm_cb"]       = "src.nm_cb",
                    ["nm_cob"]      = "src.nm_cob",
                    ["nm_scob"]     = "src.nm_scob",
                    ["no_pol_ttg"]  = "src.no_pol_ttg",
                    ["no_pol_pas"]  = "src.no_pol_pas",
                    ["no_rsk"]      = "src.no_rsk",
                    ["nm_ttg"]      = "src.nm_ttg",
                    ["ket_oby"]     = "src.ket_oby",
                    ["tgl_mul_ptg"] = "src.tgl_mul_ptg",
                    ["tgl_akh_ptg"] = "src.tgl_akh_ptg",
                    ["tgl_closing"] = "src.tgl_closing"
                },
                
                SearchableColumns = new List<string>
                {
                    "src.nm_cb",
                    "src.nm_cob",
                    "src.nm_scob",
                    "src.no_pol_ttg",
                    "src.no_pol_pas",
                    "src.nm_ttg",
                    "src.no_rsk",
                    "src.ket_oby"
                }
            };
        }
    }
}