using System.Collections.Generic;
using ABB.Application.Common.Grids.Models;

namespace ABB.Application.KontrakTreatyKeluars.Configs
{
    public static class DetailKontrakTreatyKeluarTableOfLimitConfig
    {
        public static GridConfig Create()
        {
            return new GridConfig
            {
                FromSql = @"
                            FROM (
                                SELECT 
                                    RTRIM(p.kd_cb) + RTRIM(p.kd_jns_sor) + RTRIM(p.kd_tty_pps) + RTRIM(p.kd_okup) + RTRIM(p.category_rsk) + RTRIM(p.kd_kls_konstr) AS Id,
                                    p.*,
                                    CASE RTRIM(p.category_rsk)
                                            WHEN '1' THEN 'I-LOW'
                                            WHEN '2' THEN 'II-MEDIUM'
                                            WHEN '3' THEN 'III-HIGHT'
                                            ELSE ''
                                        END as nm_category_rsk,
                                    okupasi.nm_okup,
                                    konstr.nm_kls_konstr
                                FROM ri01td04 p
                                INNER JOIN rf11 okupasi ON p.kd_okup = okupasi.kd_okup
                                INNER JOIN rf13 konstr ON p.kd_kls_konstr = konstr.kd_kls_konstr
                            ) src
                            ",
                
                BaseWhere = "(src.kd_cb = @kd_cb AND src.kd_jns_sor = @kd_jns_sor AND src.kd_tty_pps = @kd_tty_pps)",


                ColumnMap = new Dictionary<string, string>
                {
                    ["nm_category_rsk"]       = "src.nm_category_rsk",
                    ["nm_okup"]     = "src.nm_okup",
                    ["nm_kls_konstr"]     = "src.nm_kls_konstr",
                    ["pst_bts_tty"]     = "src.pst_bts_tty"
                },
                
                SearchableColumns = new List<string>
                {
                    "src.nm_category_rsk",
                    "src.nm_okup",
                    "src.nm_kls_konstr",
                    "src.pst_bts_tty"
                }
            };
        }
    }
}