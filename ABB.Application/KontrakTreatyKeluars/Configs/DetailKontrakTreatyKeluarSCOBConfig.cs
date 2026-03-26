using System.Collections.Generic;
using ABB.Application.Common.Grids.Models;

namespace ABB.Application.KontrakTreatyKeluars.Configs
{
    public static class DetailKontrakTreatyKeluarSCOBConfig
    {
        public static GridConfig Create()
        {
            return new GridConfig
            {
                FromSql = @"
                            FROM (
                                SELECT 
                                    RTRIM(p.kd_cb) + RTRIM(p.kd_jns_sor) + RTRIM(p.kd_tty_pps) + RTRIM(p.kd_cob) + RTRIM(p.kd_scob) AS Id,
                                    p.*,
                                    scob.nm_scob
                                FROM ri01td02 p
                                INNER JOIN rf05 scob ON p.kd_cob = scob.kd_cob 
                                                    AND p.kd_scob = scob.kd_scob
                            ) src
                            ",
                
                BaseWhere = "(src.kd_cb = @kd_cb AND src.kd_jns_sor = @kd_jns_sor AND src.kd_tty_pps = @kd_tty_pps)",


                ColumnMap = new Dictionary<string, string>
                {
                    ["nm_scob"]       = "src.nm_scob"
                },
                
                SearchableColumns = new List<string>
                {
                    "src.nm_scob"
                }
            };
        }
    }
}