using System.Collections.Generic;
using ABB.Application.Common.Grids.Models;

namespace ABB.Application.ProsesAlokasiKlaimReasuransis.Configs
{
    public static class ProsesAlokasiKlaimReasuransiConfig
    {
        public static GridConfig Create()
        {
            return new GridConfig
            {
                FromSql = @"
                            FROM (SELECT 
                                p.flag_closing, 
                                p.tgl_mts,
                                p.no_pol_lama,
                                p.tgl_closing,
                                p.tgl_reas,
                                p.flag_reas,
                                p.nm_ttg,
                                p.kd_cb, 
                                p.kd_cob, 
                                p.kd_scob, 
                                p.kd_thn, 
                                p.no_kl, 
                                p.no_mts, 
                                p.nilai_ttl_kl, 
                                p.tipe_mts,
                                RTRIM(p.kd_cb) + '-' + RTRIM(p.kd_cob) + '-' + RTRIM(p.kd_scob) + '-' + 
                                RTRIM(p.kd_thn) + '-' + RTRIM(p.no_kl) + '-' + CAST(p.no_mts AS VARCHAR) AS Id,
                                cb.nm_cb,
                                cob.nm_cob,
                                scob.nm_scob,
                                COALESCE(c5.total_nilai_kl_cl05, 0) AS nilai_total_klaim,
                                (p.nilai_ttl_kl - COALESCE(c5.total_nilai_kl_cl05, 0)) AS sisa_alokasi,
                                CASE RTRIM(p.tipe_mts)
                                    WHEN 'P' THEN 'PLA'
                                    WHEN 'D' THEN 'DLA'
                                    WHEN 'B' THEN 'Beban'
                                    WHEN 'R' THEN 'Recovery'
                                    ELSE ''
                                END AS nm_tipe_mts,
                                'K.' + RTRIM(p.kd_cb) + '.' + RTRIM(p.kd_scob) + '.' + 
                                RTRIM(p.kd_thn) + '.' + RTRIM(p.no_kl) AS nomor_register
                            FROM v_cl03 p
                            INNER JOIN rf01 cb ON p.kd_cb = cb.kd_cb
                            INNER JOIN rf04 cob ON p.kd_cob = cob.kd_cob
                            INNER JOIN rf05 scob ON p.kd_cob = scob.kd_cob 
                                                AND p.kd_scob = scob.kd_scob
                            LEFT JOIN (
                                SELECT 
                                    kd_cb, kd_cob, kd_scob, kd_thn, no_kl, no_mts,
                                    SUM(nilai_kl) as total_nilai_kl_cl05
                                FROM cl05
                                GROUP BY kd_cb, kd_cob, kd_scob, kd_thn, no_kl, no_mts
                            ) c5 ON p.kd_cb = c5.kd_cb 
                                AND p.kd_cob = c5.kd_cob 
                                AND p.kd_scob = c5.kd_scob 
                                AND p.kd_thn = c5.kd_thn 
                                AND p.no_kl = c5.no_kl 
                                AND p.no_mts = c5.no_mts
                                ) as src
                            ",
                
               BaseWhere = @"src.flag_closing = 'Y' AND src.flag_reas = 'N'",


                ColumnMap = new Dictionary<string, string>
                {
                    ["nm_cb"]       = "src.nm_cb",
                    ["nm_cob"]      = "src.nm_cob",
                    ["nm_scob"]     = "src.nm_scob",
                    ["no_pol_lama"]  = "src.no_pol_lama",
                    ["no_mts"]  = "src.no_mts",
                    ["nm_ttg"]      = "src.nm_ttg",
                    ["tgl_mts"] = "src.tgl_mts",
                    ["tgl_closing"] = "src.tgl_closing",
                    ["nm_tipe_mts"] = "src.nm_tipe_mts",
                    ["nomor_register"] = "src.nomor_register",
                    ["nilai_total_klaim"] = "src.nilai_total_klaim",
                    ["sisa_alokasi"] = "src.sisa_alokasi",
                    ["nilai_ttl_kl"] = "src.nilai_ttl_kl",
                    ["tgl_reas"] = "src.tgl_reas",
                    ["flag_reas"] = "src.flag_reas"
                },
                
                SearchableColumns = new List<string>
                {
                    "src.nm_cb",
                    "src.nm_cob",
                    "src.nm_scob",
                    "src.no_pol_lama",
                    "src.no_mts",
                    "src.nm_ttg",
                    "src.tgl_mts",
                    "src.tgl_closing",
                    "src.nm_tipe_mts",
                    "src.nomor_register",
                    "src.nilai_total_klaim",
                    "src.sisa_alokasi",
                    "src.nilai_ttl_kl",
                    "src.tgl_reas",
                    "src.flag_reas"
                }
            };
        }
    }
}