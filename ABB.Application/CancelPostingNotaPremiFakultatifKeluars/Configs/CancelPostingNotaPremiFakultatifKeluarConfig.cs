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
                                    pp.nm_ttg,
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
                                    INNER JOIN uw01e pp
                                        ON pp.kd_cb = p.kd_cb_pol
                                        AND pp.kd_cob = p.kd_cob
                                        AND pp.kd_scob = p.kd_scob
                                        AND pp.kd_thn = p.kd_thn
                                        AND pp.no_pol = p.no_pol
                                        AND pp.no_updt = p.no_updt
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