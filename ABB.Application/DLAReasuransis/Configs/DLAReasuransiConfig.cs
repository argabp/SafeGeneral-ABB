using System.Collections.Generic;
using ABB.Application.Common.Grids.Models;

namespace ABB.Application.DLAReasuransis.Configs
{
    public static class DLAReasuransiConfig
    {
        public static GridConfig Create()
        {
            return new GridConfig
            {
                FromSql = @"
                            FROM (
                                SELECT 
                                    RTRIM(p.kd_cb) + '-' + RTRIM(p.kd_cob) + '-' + RTRIM(p.kd_scob) + '-' + 
                                    RTRIM(p.kd_thn) + '-' + RTRIM(p.no_kl) + '-' + CAST(p.no_mts AS VARCHAR) + '-' + 
                                    CAST(p.no_dla AS VARCHAR) AS Id,
                                    p.*,
                                    cb.nm_cb,
                                    cob.nm_cob,
                                    scob.nm_scob,
                                    'K.' + RTRIM(p.kd_cb) + '.' + RTRIM(p.kd_scob) + '.' + 
                                    RTRIM(p.kd_thn) + '.' + RTRIM(p.no_kl) as nomor_register
                                FROM cl09r p
                                INNER JOIN rf01 cb ON p.kd_cb = cb.kd_cb
                                INNER JOIN rf04 cob ON p.kd_cob = cob.kd_cob
                                INNER JOIN rf05 scob ON p.kd_cob = scob.kd_cob 
                                                    AND p.kd_scob = scob.kd_scob
                            ) src
                            ",
                
               BaseWhere = "",


                ColumnMap = new Dictionary<string, string>
                {
                    ["nm_cb"]       = "src.nm_cb",
                    ["nm_cob"]      = "src.nm_cob",
                    ["nm_scob"]     = "src.nm_scob",
                    ["nm_grp_pas"] = "src.nm_grp_pas",
                    ["nm_rk_pas"] = "src.nm_rk_pas"
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