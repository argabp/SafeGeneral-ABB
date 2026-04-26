using System.Collections.Generic;
using ABB.Application.Common.Grids.Models;

namespace ABB.Application.Alokasis.Configs
{
    public static class AlokasiConfig
    {
        public static GridConfig Create()
        {
            return new GridConfig
            {
                FromSql = @"
                    FROM (  
                        SELECT DISTINCT
                            CAST(BINARY_CHECKSUM(p.kd_cb, p.kd_cob, p.kd_scob, p.kd_thn, p.no_pol, p.no_updt) AS BIGINT) AS Id,
                            p.*,
                            m.nm_mtu nm_mtu_prm
                        FROM ri01e p
                        INNER JOIN rf06 m ON p.kd_mtu_prm = m.kd_mtu
                    ) as src",
                
                BaseWhere = "(src.kd_cb = @kd_cb AND src.kd_cob = @kd_cob AND" +
                            " src.kd_scob = @kd_scob AND src.kd_thn = @kd_thn AND" +
                            " src.no_pol = @no_pol AND src.no_updt = @no_updt)",

                ColumnMap = new Dictionary<string, string>
                {
                    ["no_rsk"] = "src.no_rsk",
                    ["kd_endt"] = "src.kd_endt",
                    ["no_updt_reas"] = "src.no_updt_reas",
                    ["kd_mtu_reas"] = "src.kd_mtu_reas",
                    ["nilai_ttl_ptg"] = "src.nilai_ttl_ptg",
                    ["nilai_prm"] = "src.nilai_prm"
                },

                SearchableColumns = new List<string>
                {
                    "src.no_rsk",
                    "src.kd_endt",
                    "src.no_updt_reas",
                    "src.kd_mtu_reas",
                    "src.nilai_ttl_ptg",
                    "src.nilai_prm"
                }
            };
        }
    }
}