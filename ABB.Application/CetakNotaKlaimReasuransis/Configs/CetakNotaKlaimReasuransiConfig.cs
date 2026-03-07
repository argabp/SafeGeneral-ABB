using System.Collections.Generic;
using ABB.Application.Common.Grids.Models;

namespace ABB.Application.CetakNotaKlaimReasuransis.Configs
{
    public static class CetakNotaKlaimReasuransiConfig
    {
        public static GridConfig Create()
        {
            return new GridConfig
            {
                FromSql = @"
                            FROM (
                                SELECT 
                                    p.*,
                                    cb.nm_cb,
                                    cob.nm_cob,
                                    scob.nm_scob,
                                    CASE RTRIM(p.tipe_mts)
                                            WHEN 'P' THEN 'PLA'
                                            WHEN 'D' THEN 'DLA'
                                            WHEN 'B' THEN 'Beban'
                                            WHEN 'R' THEN 'Recovery'
                                            ELSE ''
                                        END as nm_tipe_mts,
                                     RTRIM(p.kd_cb) + '.' + RTRIM(p.jns_tr) + '.' + 
                                     RTRIM(p.jns_nt_msk) + '.' + RTRIM(p.kd_thn) + '.' +  RTRIM(p.kd_bln) + '.' + 
                                     RTRIM(p.no_nt_msk) + '.' + RTRIM(p.jns_nt_kel) + '.' +  RTRIM(p.no_nt_kel) as nomor_nota,
                                     RTRIM(p.kd_cb) + '.' + RTRIM(p.kd_cob) + '.' + 
                                     RTRIM(p.kd_scob) + '.' + RTRIM(p.kd_thn) + '.' +  RTRIM(p.no_kl) + '.' + 
                                     RTRIM(p.no_mts) as nomor_berkas
                                 FROM cl10rp p
                                    INNER JOIN rf01 cb ON p.kd_cb = cb.kd_cb
                                    INNER JOIN rf04 cob ON p.kd_cob = cob.kd_cob
                                    INNER JOIN rf05 scob ON p.kd_cob = scob.kd_cob 
                                                        AND p.kd_scob = scob.kd_scob
                            ) src
                            ",

                BaseWhere = @"
                            (src.tipe_mts <> 'P')
                            ",
                
                ColumnMap = new Dictionary<string, string>
                {
                    ["nomor_nota"] = "src.nomor_nota",
                    ["nomor_berkas"] = "src.nomor_berkas",
                    ["nm_cb"] = "src.nm_cb",
                    ["nm_cob"] = "src.nm_cob",
                    ["nm_scob"] = "src.nm_scob",
                    ["nm_ttj"] = "src.nm_ttj",
                    ["flag_posting"] = "src.flag_posting",
                    ["nm_tipe_mts"] = "src.nm_tipe_mts",
                    ["nilai_ttl_kl"] = "src.nilai_ttl_kl",
                    ["tgl_closing"] = "src.tgl_closing",
                },

                SearchableColumns = new List<string>
                {
                    "src.nomor_nota",
                    "src.nomor_berkas",
                    "src.nm_cb",
                    "src.nm_cob",
                    "src.nm_scob",
                    "src.nm_ttj",
                    "src.flag_posting",
                    "src.nm_tipe_mts",
                    "src.nilai_ttl_kl",
                    "src.tgl_closing"
                }
            };
        }
    }
}