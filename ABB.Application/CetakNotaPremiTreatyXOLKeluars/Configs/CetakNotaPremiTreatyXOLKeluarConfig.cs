using System.Collections.Generic;
using ABB.Application.Common.Grids.Models;

namespace ABB.Application.CetakNotaPremiTreatyXOLKeluars.Configs
{
    public static class CetakNotaPremiTreatyXOLKeluarConfig
    {
        public static GridConfig Create()
        {
            return new GridConfig
            {
                FromSql = @"
                            FROM (
                                SELECT 
                                    p.*,
                                    cob.nm_cob,
                                    mtu.nm_mtu + '(' + mtu.symbol + ')' nm_mtu,
                                     RTRIM(p.jns_sb_nt) + '.' + RTRIM(p.kd_cb) + '.' + RTRIM(p.jns_tr) + '.' + 
                                     RTRIM(p.jns_nt_msk) + '.' + RTRIM(p.kd_thn) + '.' +  RTRIM(p.kd_bln) + '.' + 
                                     RTRIM(p.no_nt_msk) + '.' + RTRIM(p.jns_nt_kel) + '.' +  RTRIM(p.no_nt_kel) as Id,
                                     RTRIM(p.kd_cb) + '.' + RTRIM(p.jns_tr) + '.' + 
                                     RTRIM(p.jns_nt_msk) + '.' + RTRIM(p.kd_thn) + '.' +  RTRIM(p.kd_bln) + '.' + 
                                     RTRIM(p.no_nt_msk) + '.' + RTRIM(p.jns_nt_kel) + '.' +  RTRIM(p.no_nt_kel) as nomor_nota
                                 FROM fn05 p
                                    INNER JOIN rf04 cob ON p.kd_cob = cob.kd_cob
                                    INNER JOIN rf06 mtu ON p.kd_mtu = mtu.kd_mtu
                            ) src
                            ",

                BaseWhere = @"
                            (src.jns_tr = 'P' AND src.jns_nt_msk = '0' AND src.jns_nt_kel = 'N')
                            ",

                ColumnMap = new Dictionary<string, string>
                {
                    ["nomor_nota"] = "src.nomor_nota",
                    ["nm_ttj"] = "src.nm_ttj",
                    ["nm_cob"] = "src.nm_cob",
                    ["nm_mtu"] = "src.nm_mtu",
                    ["ket_nt"] = "src.ket_nt",
                    ["nilai_nt"] = "src.nilai_nt",
                    ["tgl_nt"] = "src.tgl_nt",
                    ["flag_posting"] = "src.flag_posting"
                },

                SearchableColumns = new List<string>
                {
                    "src.nomor_nota",
                    "src.nm_ttj",
                    "src.nm_cob",
                    "src.nm_mtu",
                    "src.ket_nt",
                    "src.nilai_nt",
                    "src.tgl_nt",
                    "src.flag_posting"
                }
            };
        }
    }
}