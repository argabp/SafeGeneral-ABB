using System.Collections.Generic;
using ABB.Application.Common.Grids.Models;

namespace ABB.Application.PostingNotaPremiXOLKeluars.Configs
{
    public static class PostingNotaPremiXOLKeluarConfig
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
                                    m.nm_mtu,
                                    p.jns_sb_nt + '-' + 
                                    p.kd_cb + '-' + 
                                    p.jns_tr + '-' + 
                                    p.jns_nt_msk + '-' + 
                                    p.kd_thn + '-' + 
                                    p.kd_bln + '-' + 
                                    p.no_nt_msk + '-' + 
                                    p.jns_nt_kel + '-' + 
                                    p.no_nt_kel AS Id,
                                    RTRIM(p.kd_cb) + '.' + RTRIM(p.jns_tr) + '.' + 
                                    RTRIM(p.jns_nt_msk) + '.' + RTRIM(p.kd_thn) + '.' +  RTRIM(p.kd_bln) + '.' + 
                                    RTRIM(p.no_nt_msk) + '.' + RTRIM(p.jns_nt_kel) + '.' +  RTRIM(p.no_nt_kel) as nomor_nota
                                 FROM fn05 p
                                    INNER JOIN rf04 cob ON p.kd_cob = cob.kd_cob
                                    INNER JOIN rf06 m ON p.kd_mtu = m.kd_mtu
                            ) src
                            ",

                BaseWhere = @"(src.flag_posting = 'N' AND src.jns_tr = 'P' 
                                AND src.jns_nt_msk = '0' AND src.jns_nt_kel = 'N'
                                AND src.jns_nt_kel <> 'C')",

                ColumnMap = new Dictionary<string, string>
                {
                    ["nomor_nota"] = "src.nomor_nota",
                    ["nm_cob"] = "src.nm_cob",
                    ["nm_mtu"] = "src.nm_mtu",
                    ["nm_ttj"] = "src.nm_ttj",
                    ["ket_nt"] = "src.ket_nt",
                    ["tgl_nt"] = "src.tgl_nt",
                    ["nilai_nt"] = "src.nilai_nt"
                },

                SearchableColumns = new List<string>
                {
                    "src.nomor_nota",
                    "src.nm_cob",
                    "src.nm_mtu",
                    "src.nm_ttj",
                    "src.ket_nt",
                    "src.tgl_nt",
                    "src.nilai_nt"
                }
            };
        }
    }
}