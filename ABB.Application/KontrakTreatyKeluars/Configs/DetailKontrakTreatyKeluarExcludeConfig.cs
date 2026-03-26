using System.Collections.Generic;
using ABB.Application.Common.Grids.Models;

namespace ABB.Application.KontrakTreatyKeluars.Configs
{
    public static class DetailKontrakTreatyKeluarExcludeConfig
    {
        public static GridConfig Create()
        {
            return new GridConfig
            {
                FromSql = @"
                            FROM (
                                SELECT 
                                    RTRIM(p.kd_cb) + RTRIM(p.kd_jns_sor) + RTRIM(p.kd_tty_pps) + RTRIM(p.kd_okup) AS Id,
                                    p.*,
                                    okupasi.nm_okup
                                FROM ri01td06 p
                                INNER JOIN rf11 okupasi ON p.kd_okup = okupasi.kd_okup
                            ) src
                            ",
                
                BaseWhere = "(src.kd_cb = @kd_cb AND src.kd_jns_sor = @kd_jns_sor AND src.kd_tty_pps = @kd_tty_pps)",


                ColumnMap = new Dictionary<string, string>
                {
                    ["nm_okup"]       = "src.nm_okup"
                },
                
                SearchableColumns = new List<string>
                {
                    "src.nm_okup"
                }
            };
        }
    }
}