using System.Collections.Generic;
using ABB.Application.Common.Grids.Models;

namespace ABB.Application.CancelPostingNotaPremiTreatyKeluars.Configs
{
    public static class CancelPostingNotaPremiTreatyKeluarConfig
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
                                    cob.nm_cob,
                                    m.nm_mtu,
                                     RTRIM(p.kd_cb) + '.' + RTRIM(p.jns_tr) + '.' + 
                                     RTRIM(p.jns_nt_msk) + '.' + RTRIM(p.kd_thn) + '.' +  RTRIM(p.kd_bln) + '.' + 
                                     RTRIM(p.no_nt_msk) + '.' + RTRIM(p.jns_nt_kel) + '.' +  RTRIM(p.no_nt_kel) as nomor_nota
                                 FROM ri07e p
                                    INNER JOIN rf03 r 
                                        ON p.kd_rk_pas = r.kd_rk
                                            AND p.kd_grp_pas = r.kd_grp_rk
                                            AND p.kd_cb = r.kd_cb
                                    INNER JOIN rf04 cob ON p.kd_cob = cob.kd_cob
                                    INNER JOIN rf06 m ON p.kd_mtu = m.kd_mtu
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