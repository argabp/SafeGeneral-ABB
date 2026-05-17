using System.Collections.Generic;
using ABB.Application.Common.Grids.Models;

namespace ABB.Application.KontrakTreatyMasukXOLs.Configs
{
    public static class KontrakTreatyMasukXOLConfig
    {
        public static GridConfig Create()
        {
            return new GridConfig
            {
                FromSql = @"
                            FROM (
                                SELECT 
                                    RTRIM(p.kd_cb) + '-' + RTRIM(p.kd_jns_sor) + '-' + RTRIM(p.kd_tty_msk) AS Id,
                                    p.*,
                                    s.nm_jns_sor
                                FROM ri01i p
                                INNER JOIN rf18 s ON p.kd_jns_sor = s.kd_jns_sor
                            ) src
                            ",

                BaseWhere = "(src.kd_jns_sor = 'XOL')",

                ColumnMap = new Dictionary<string, string>
                {
                    ["thn_uw"]       = "src.thn_uw",
                    ["nm_jns_sor"]      = "src.nm_jns_sor",
                    ["kd_tty_msk"]     = "src.kd_tty_msk",
                    ["desk_tty"]     = "src.desk_tty"
                },
                
                SearchableColumns = new List<string>
                {
                    "src.thn_uw",
                    "src.nm_jns_sor",
                    "src.kd_tty_msk",
                    "src.desk_tty"
                }
            };
        }
    }
}