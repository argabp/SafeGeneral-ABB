using System.Collections.Generic;
using ABB.Application.Common.Grids.Models;

namespace ABB.Application.KontrakTreatyKeluars.Configs
{
    public static class DetailKontrakTreatyKeluarConfig
    {
        public static GridConfig Create()
        {
            return new GridConfig
            {
                FromSql = @"
                            FROM (
                                SELECT 
                                    RTRIM(p.kd_cb) + RTRIM(p.kd_jns_sor) + RTRIM(p.kd_tty_pps) + RTRIM(p.kd_grp_pas) + RTRIM(p.kd_rk_pas) AS Id,
                                    p.*,
                                    CASE RTRIM(p.kd_grp_sb_bis)
                                            WHEN '2' THEN 'Broker'
                                            WHEN '5' THEN '-'
                                            ELSE ''
                                        END as nm_grp_sb_bis,
                                    rekanan1.nm_rk nm_rk_sb_bis,
                                    CASE RTRIM(p.kd_grp_pas)
                                            WHEN '5' THEN 'PAS / REAS'
                                            ELSE ''
                                        END as nm_grp_pas,
                                    rekanan2.nm_rk nm_rk_pas
                                FROM ri01td01 p
                                INNER JOIN rf03 rekanan1 ON p.kd_cb = rekanan1.kd_cb
                                                            AND p.kd_rk_sb_bis = rekanan1.kd_rk
															AND p.kd_grp_sb_bis = rekanan1.kd_grp_rk
                                INNER JOIN rf03 rekanan2 ON p.kd_cb = rekanan2.kd_cb
                                                            AND p.kd_rk_pas = rekanan2.kd_rk
															AND p.kd_grp_pas = rekanan2.kd_grp_rk
                            ) src
                            ",
                
                BaseWhere = "(src.kd_cb = @kd_cb AND src.kd_jns_sor = @kd_jns_sor AND src.kd_tty_pps = @kd_tty_pps)",


                ColumnMap = new Dictionary<string, string>
                {
                    ["nm_grp_sb_bis"]       = "src.nm_grp_sb_bis",
                    ["nm_rk_sb_bis"]      = "src.nm_rk_sb_bis",
                    ["nm_grp_pas"]     = "src.nm_grp_pas",
                    ["nm_rk_pas"]     = "src.nm_rk_pas",
                    ["pst_share"]     = "src.pst_share",
                    ["pst_com"]     = "src.pst_com",
                },
                
                SearchableColumns = new List<string>
                {
                    "src.nm_grp_sb_bis",
                    "src.nm_rk_sb_bis",
                    "src.nm_grp_pas",
                    "src.nm_rk_pas",
                    "src.pst_share",
                    "src.pst_com"
                }
            };
        }
    }
}