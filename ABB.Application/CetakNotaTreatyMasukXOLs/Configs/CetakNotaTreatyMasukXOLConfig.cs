using System.Collections.Generic;
using ABB.Application.Common.Grids.Models;

namespace ABB.Application.CetakNotaTreatyMasukXOLs.Configs
{
    public static class CetakNotaTreatyMasukXOLConfig
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
                                    m.nm_mtu,
                                     RTRIM(p.kd_cb) + '.' + RTRIM(p.jns_tr) + '.' + 
                                     RTRIM(p.jns_nt_msk) + '.' + RTRIM(p.kd_thn) + '.' +  RTRIM(p.kd_bln) + '.' + 
                                     RTRIM(p.no_nt_msk) + '.' + RTRIM(p.jns_nt_kel) + '.' +  RTRIM(p.no_nt_kel) as nomor_nota
                                 FROM v_ri02e p
                                    INNER JOIN rf06 m ON p.kd_mtu = m.kd_mtu
                                    INNER JOIN rf03 r 
                                        ON p.kd_rk_pas = r.kd_rk
                                            AND p.kd_grp_pas = r.kd_grp_rk
                                            AND p.kd_cb = r.kd_cb
                            ) src
                            ",
                
               BaseWhere = "(src.kd_jns_sor = 'XOL')",


                ColumnMap = new Dictionary<string, string>
                {
                    ["nomor_nota"]       = "src.nomor_nota",
                    ["no_ref_nt"]      = "src.no_ref_nt",
                    ["nm_ttj"]     = "src.nm_ttj",
                    ["tgl_nt"] = "src.tgl_nt",
                    ["nilai_nt"] = "src.nilai_nt",
                    ["nm_mtu"] = "src.nm_mtu"
                },
                
                SearchableColumns = new List<string>
                {
                    "src.nomor_nota",
                    "src.no_ref_nt",
                    "src.nm_ttj",
                    "src.tgl_nt",
                    "src.nilai_nt",
                    "src.nm_mtu"
                }
            };
        }
    }
}