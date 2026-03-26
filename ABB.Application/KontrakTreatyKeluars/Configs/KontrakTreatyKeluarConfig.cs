using System.Collections.Generic;
using ABB.Application.Common.Grids.Models;

namespace ABB.Application.KontrakTreatyKeluars.Configs
{
    public static class KontrakTreatyKeluarConfig
    {
        public static GridConfig Create()
        {
            return new GridConfig
            {
                FromSql = @"
                            FROM (
                                SELECT 
                                    RTRIM(p.kd_cb) + '-' + RTRIM(p.kd_jns_sor) + '-' + RTRIM(p.kd_tty_pps) AS Id,
                                    p.*,
                                    cb.nm_cb,
                                    s.nm_jns_sor
                                FROM ri01t p
                                INNER JOIN rf01 cb ON p.kd_cb = cb.kd_cb
                                INNER JOIN rf18 s ON p.kd_jns_sor = s.kd_jns_sor
                            ) src
                            ",
                
                BaseWhere = "",


                ColumnMap = new Dictionary<string, string>
                {
                    ["nm_cb"]       = "src.nm_cb",
                    ["nm_jns_sor"]      = "src.nm_jns_sor",
                    ["kd_tty_pps"]     = "src.kd_tty_pps"
                },
                
                SearchableColumns = new List<string>
                {
                    "src.nm_cb",
                    "src.nm_jns_sor",
                    "src.kd_tty_pps"
                }
            };
        }
    }
}