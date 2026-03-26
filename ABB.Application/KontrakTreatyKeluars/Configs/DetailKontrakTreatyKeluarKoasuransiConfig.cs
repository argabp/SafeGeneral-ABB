using System.Collections.Generic;
using ABB.Application.Common.Grids.Models;

namespace ABB.Application.KontrakTreatyKeluars.Configs
{
    public static class DetailKontrakTreatyKeluarKoasuransiConfig
    {
        public static GridConfig Create()
        {
            return new GridConfig
            {
                FromSql = @"
                            FROM (
                                SELECT 
                                    RTRIM(p.kd_cb) + RTRIM(p.kd_jns_sor) + RTRIM(p.kd_tty_pps) + RTRIM(p.no_urut) AS Id,
                                    p.*
                                FROM ri01td05 p
                            ) src
                            ",
                
                BaseWhere = "(src.kd_cb = @kd_cb AND src.kd_jns_sor = @kd_jns_sor AND src.kd_tty_pps = @kd_tty_pps)",


                ColumnMap = new Dictionary<string, string>
                {
                    ["no_urut"]       = "src.no_urut",
                    ["pst_share_mul"]     = "src.pst_share_mul",
                    ["pst_share_akh"]     = "src.pst_share_akh",
                    ["pst_bts_koas"]     = "src.pst_bts_koas"
                },
                
                SearchableColumns = new List<string>
                {
                    "src.no_urut",
                    "src.pst_share_mul",
                    "src.pst_share_akh",
                    "src.pst_bts_koas"
                }
            };
        }
    }
}