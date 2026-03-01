using System.Collections.Generic;
using ABB.Application.Common.Grids.Models;

namespace ABB.Application.EntriNotaKlaimTreaties.Configs
{
    public static class EntriNotaKlaimTreatyConfig
    {
        public static GridConfig Create()
        {
            return new GridConfig
            {
                FromSql = @"
                            FROM (
                                SELECT 
                                    p.*,
                                    r.nm_rk,
                                    cob.nm_cob,
                                    mtu.nm_mtu + '(' + mtu.symbol + ')' nm_mtu,
                                     RTRIM(p.kd_cb) + '.' + RTRIM(p.jns_tr) + '.' + 
                                     RTRIM(p.jns_nt_msk) + '.' + RTRIM(p.kd_thn) + '.' +  RTRIM(p.kd_bln) + '.' + 
                                     RTRIM(p.no_nt_msk) + '.' + RTRIM(p.jns_nt_kel) + '.' +  RTRIM(p.no_nt_kel) as nomor_nota
                                 FROM ri09e p
                                    INNER JOIN rf04 cob ON p.kd_cob = cob.kd_cob
                                    INNER JOIN rf06 mtu ON p.kd_mtu = mtu.kd_mtu
                                    LEFT JOIN rf03 r 
                                        ON p.kd_rk_pas = r.kd_rk
                                            AND p.kd_grp_pas = r.kd_grp_rk
                                            AND p.kd_cb = r.kd_cb
                            ) src
                            ",

                BaseWhere = @"
                            (src.flag_cancel = 'N')
                            ",

                ColumnMap = new Dictionary<string, string>
                {
                    ["nomor_nota"] = "src.nomor_nota",
                    ["nm_rk"] = "src.nm_rk",
                    ["nm_cob"] = "src.nm_cob",
                    ["nm_mtu"] = "src.nm_mtu",
                    ["ket_nt"] = "src.ket_nt",
                    ["nilai_nt"] = "src.nilai_nt",
                    ["tgl_nt"] = "src.tgl_nt",
                    ["flag_posting"] = "src.flag_posting",
                    ["flag_cancel"] = "src.flag_cancel"
                },

                SearchableColumns = new List<string>
                {
                    "src.nomor_nota",
                    "src.nm_rk",
                    "src.nm_cob",
                    "src.nm_mtu",
                    "src.ket_nt",
                    "src.nilai_nt",
                    "src.tgl_nt",
                    "src.flag_posting",
                    "src.flag_cancel"
                }
            };
        }
    }
}