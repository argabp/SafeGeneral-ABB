using System.Collections.Generic;
using ABB.Application.Common.Grids.Models;

namespace ABB.Application.KontrakTreatyKeluars.Configs
{
    public static class DetailKontrakTreatyKeluarCoverageConfig
    {
        public static GridConfig Create()
        {
            return new GridConfig
            {
                FromSql = @"
                            FROM (
                                SELECT 
                                    RTRIM(p.kd_cb) + RTRIM(p.kd_jns_sor) + RTRIM(p.kd_tty_pps) + RTRIM(p.kd_cvrg) AS Id,
                                    p.*,
                                    coverage.nm_cvrg
                                FROM ri01td03 p
                                INNER JOIN rf17 coverage ON p.kd_cvrg = coverage.kd_cvrg
                            ) src
                            ",
                
                BaseWhere = "(src.kd_cb = @kd_cb AND src.kd_jns_sor = @kd_jns_sor AND src.kd_tty_pps = @kd_tty_pps)",


                ColumnMap = new Dictionary<string, string>
                {
                    ["nm_cvrg"]       = "src.nm_cvrg",
                    ["pst_kms_reas"]     = "src.pst_kms_reas",
                    ["max_limit_jktb"]     = "src.max_limit_jktb",
                    ["max_limit_prov"]     = "src.max_limit_prov"
                },
                
                SearchableColumns = new List<string>
                {
                    "src.nm_cvrg",
                    "src.pst_kms_reas",
                    "src.max_limit_jktb",
                    "src.max_limit_prov"
                }
            };
        }
    }
}