using System.Collections.Generic;
using ABB.Application.Common.Grids.Models;

namespace ABB.Application.PostingNotaPremiFakultatifKeluars.Configs
{
    public static class PostingNotaPremiFakultatifKeluarConfig
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
                                    scob.nm_scob,
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
                                 FROM ri04e p
                                    INNER JOIN rf04 cob ON p.kd_cob = cob.kd_cob
                                    INNER JOIN rf05 scob ON p.kd_cob = scob.kd_cob 
                                                        AND p.kd_scob = scob.kd_scob
                            ) src
                            ",

                BaseWhere = @"(src.flag_cancel = 'N' AND src.flag_posting = 'N')",

                ColumnMap = new Dictionary<string, string>
                {
                    ["nomor_nota"] = "src.nomor_nota",
                    ["nm_cb"] = "src.nm_cb",
                    ["nm_cob"] = "src.nm_cob",
                    ["nm_scob"] = "src.nm_scob",
                    ["no_pol"] = "src.no_pol",
                    ["no_rsk"] = "src.no_rsk",
                    ["nm_ttj"] = "src.nm_ttj",
                    ["nm_ttg"] = "src.nm_ttj",
                    ["tgl_nt"] = "src.tgl_nt",
                    ["nilai_nt"] = "src.nilai_nt"
                },

                SearchableColumns = new List<string>
                {
                    "src.nomor_nota",
                    "src.nm_cb",
                    "src.nm_cob",
                    "src.nm_scob",
                    "src.no_pol",
                    "src.no_rsk",
                    "src.nm_ttj",
                    "src.nm_ttg",
                    "src.tgl_nt",
                    "src.nilai_nt"
                }
            };
        }
    }
}