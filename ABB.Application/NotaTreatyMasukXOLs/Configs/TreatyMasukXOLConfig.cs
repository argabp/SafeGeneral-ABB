using System.Collections.Generic;
using ABB.Application.Common.Grids.Models;

namespace ABB.Application.NotaTreatyMasukXOLs.Configs
{
    public static class TreatyMasukXOLConfig
    {
        public static GridConfig Create()
        {
            return new GridConfig
            {
                FromSql = @"
                            FROM (
                                SELECT 
                                    RTRIM(p.kd_cb) + RTRIM(p.kd_jns_sor) + RTRIM(p.kd_tty_msk) AS Id,
                                    p.*,
                                    CASE RTRIM(p.tipe_tty_msk)
                                            WHEN 'S' THEN 'Sesi'
                                            WHEN 'R' THEN 'Retrosesi'
                                            ELSE ''
                                        END as nm_tipe_tty_msk,
                                    cob.nm_cob,
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
                                FROM ri01i p
                                INNER JOIN rf04 cob ON p.kd_cob = cob.kd_cob
                                INNER JOIN rf03 rekanan1 ON p.kd_cb = rekanan1.kd_cb
                                                            AND p.kd_rk_sb_bis = rekanan1.kd_rk
															AND p.kd_grp_sb_bis = rekanan1.kd_grp_rk
                                INNER JOIN rf03 rekanan2 ON p.kd_cb = rekanan2.kd_cb
                                                            AND p.kd_rk_pas = rekanan2.kd_rk
															AND p.kd_grp_pas = rekanan2.kd_grp_rk
                            ) src
                            ",
                
                BaseWhere = "(src.kd_jns_sor = @kd_jns_sor AND src.kd_cb = @kd_cb)",


                ColumnMap = new Dictionary<string, string>
                {
                    ["nm_cob"]       = "src.nm_cob",
                    ["kd_tty_msk"]       = "src.kd_tty_msk",
                    ["nm_tipe_tty_msk"]       = "src.nm_tipe_tty_msk",
                    ["thn_uw"]       = "src.thn_uw",
                    ["nm_grp_sb_bis"]       = "src.nm_grp_sb_bis",
                    ["nm_rk_sb_bis"]       = "src.nm_rk_sb_bis",
                    ["nm_grp_pas"]       = "src.nm_grp_pas",
                    ["nm_rk_pas"]       = "src.nm_rk_pas",
                    ["desk_tty"]       = "src.desk_tty",
                    ["pst_share"]       = "src.pst_share"
                },
                
                SearchableColumns = new List<string>
                {
                    "src.nm_cob",
                    "src.kd_tty_msk",
                    "src.nm_tipe_tty_msk",
                    "src.thn_uw",
                    "src.nm_grp_sb_bis",
                    "src.nm_rk_sb_bis",
                    "src.nm_grp_pas",
                    "src.nm_rk_pas",
                    "src.desk_tty",
                    "src.pst_share"
                }
            };
        }
    }
}