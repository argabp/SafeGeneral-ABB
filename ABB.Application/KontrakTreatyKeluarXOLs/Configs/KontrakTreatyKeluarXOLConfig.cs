using System.Collections.Generic;
using ABB.Application.Common.Grids.Models;

namespace ABB.Application.KontrakTreatyKeluarXOLs.Configs
{
    public static class KontrakTreatyKeluarXOLConfig
    {
        public static GridConfig Create()
        {
            return new GridConfig
            {
                FromSql = @"
                            FROM (
                                SELECT 
                                    RTRIM(p.kd_cb) + '-' + RTRIM(p.kd_jns_sor) + '-' + RTRIM(p.kd_tty_npps) AS Id,
                                    p.*,
                                    s.nm_jns_sor
                                FROM ri02t p
                                INNER JOIN rf18 s ON p.kd_jns_sor = s.kd_jns_sor
                            ) src
                            ",
                
                BaseWhere = "",
                
                ColumnMap = new Dictionary<string, string>
                {
                    ["thn_tty_npps"]       = "src.thn_tty_npps",
                    ["nm_jns_sor"]      = "src.nm_jns_sor",
                    ["kd_tty_pps"]     = "src.kd_tty_pps",
                    ["nm_tty_npps"]     = "src.nm_tty_npps"
                },
                
                SearchableColumns = new List<string>
                {
                    "src.thn_tty_npps",
                    "src.nm_jns_sor",
                    "src.kd_tty_pps",
                    "src.nm_tty_npps"
                }
            };
        }
    }
}