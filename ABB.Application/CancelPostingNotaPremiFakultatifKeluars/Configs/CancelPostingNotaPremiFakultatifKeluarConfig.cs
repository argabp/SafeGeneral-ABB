using System.Collections.Generic;
using ABB.Application.Common.Grids.Models;

namespace ABB.Application.CancelPostingNotaPremiFakultatifKeluars.Configs
{
    public static class CancelPostingNotaPremiFakultatifKeluarConfig
    {
        public static GridConfig Create()
        {
            return new GridConfig
            {
                FromSql = @"
                            FROM (
                                SELECT 
                                    p.*,
                                    r.nm_rk nm_ttj,
                                     RTRIM(p.kd_cb) + '.' + RTRIM(p.jns_tr) + '.' + 
                                     RTRIM(p.jns_nt_msk) + '.' + RTRIM(p.kd_thn) + '.' +  RTRIM(p.kd_bln) + '.' + 
                                     RTRIM(p.no_nt_msk) + '.' + RTRIM(p.jns_nt_kel) + '.' +  RTRIM(p.no_nt_kel) as id,
                                     RTRIM(p.kd_cb) + '.' + RTRIM(p.jns_tr) + '.' + 
                                     RTRIM(p.jns_nt_msk) + '.' + RTRIM(p.kd_thn) + '.' +  RTRIM(p.kd_bln) + '.' + 
                                     RTRIM(p.no_nt_msk) + '.' + RTRIM(p.jns_nt_kel) + '.' +  RTRIM(p.no_nt_kel) as nomor_nota
                                 FROM ri04e p
                                INNER JOIN rf03 r 
                                    ON p.kd_rk_pas = r.kd_rk
                                        AND p.kd_grp_pas = r.kd_grp_rk
                                        AND p.kd_cb = r.kd_cb
                            ) src
                            ",

                BaseWhere = @"
                            (src.flag_cancel = 'N' AND src.flag_posting = 'Y')
                            ",

                ColumnMap = new Dictionary<string, string>
                {
                    ["nomor_nota"] = "src.nomor_nota",
                    ["nm_ttj"] = "src.nm_ttj"
                },

                SearchableColumns = new List<string>
                {
                    "src.nomor_nota",
                    "src.nm_ttj"
                }
            };
        }
    }
}