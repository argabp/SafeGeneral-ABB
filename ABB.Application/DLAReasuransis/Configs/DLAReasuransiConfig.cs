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
                                    RTRIM(p.kd_thn) + '.' + RTRIM(p.no_kl) as nomor_berkas,
									 r.nm_rk nm_ttj, 
									 p2.nm_ttg, 
									 p3.no_pol_lama
                                FROM cl09r p
                                INNER JOIN rf01 cb ON p.kd_cb = cb.kd_cb
                                INNER JOIN rf04 cob ON p.kd_cob = cob.kd_cob
                                INNER JOIN rf05 scob ON p.kd_cob = scob.kd_cob 
                                                    AND p.kd_scob = scob.kd_scob
								INNER JOIN cl01 p3
									ON p.kd_cb = p3.kd_cb
										AND p.kd_cob=p3.kd_cob
										AND p.kd_scob=p3.kd_scob
										AND p.kd_thn =p3.kd_thn 
										AND p.no_kl=p3.no_kl
								INNER JOIN uw01e p2
									ON p3.kd_cb = p2.kd_cb
										AND p3.kd_cob=p2.kd_cob
										AND p3.kd_scob=p2.kd_scob
										AND p3.kd_thn_pol =p2.kd_thn 
										AND p3.no_pol=p2.no_pol
										AND p3.no_updt=p2.no_updt
								INNER JOIN rf03 r
									ON p.kd_cb = r.kd_cb
										AND p.kd_grp_pas = r.kd_grp_rk
										AND p.kd_rk_pas = r.kd_rk
                            ) src
                            ",
                
               BaseWhere = "",


                ColumnMap = new Dictionary<string, string>
                {
                    ["nm_cb"]       = "src.nm_cb",
                    ["nm_cob"]      = "src.nm_cob",
                    ["nm_scob"]     = "src.nm_scob",
                    ["no_pol_lama"] = "src.no_pol_lama",
                    ["nm_ttg"] = "src.nm_ttg",
                    ["nm_ttj"] = "src.nm_ttj",
                    ["nomor_berkas"] = "src.nomor_berkas",
                    ["no_mts"] = "src.no_mts",
                    ["no_dla"] = "src.no_dla"
                },
                
                SearchableColumns = new List<string>
                {
                    "src.nm_cb",
                    "src.nm_cob",
                    "src.nm_scob",
                    "src.no_pol_lama",
                    "src.nm_ttg",
                    "src.nm_ttj",
                    "src.nomor_berkas",
                    "src.no_mts",
                    "src.no_dla"
                }
            };
        }
    }
}