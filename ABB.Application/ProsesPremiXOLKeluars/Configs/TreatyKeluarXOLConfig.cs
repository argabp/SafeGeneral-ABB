using System.Collections.Generic;
using ABB.Application.Common.Grids.Models;

namespace ABB.Application.ProsesPremiXOLKeluars.Configs
{
    public static class TreatyKeluarXOLConfig
    {
        public static GridConfig Create()
        {
            return new GridConfig
            {
                FromSql = @"
                            FROM (
                                SELECT 
                                    RTRIM(p.kd_cb) + RTRIM(p.kd_jns_sor) + RTRIM(p.kd_tty_npps) AS Id,
                                    p.*,
                                    cob.nm_cob
                                FROM ri02t p
                                INNER JOIN rf04 cob ON p.kd_cob = cob.kd_cob
                            ) src
                            ",
                
                BaseWhere = "(src.kd_jns_sor = @kd_jns_sor AND src.kd_cb = @kd_cb)",


                ColumnMap = new Dictionary<string, string>
                {
                    ["nm_cob"]       = "src.nm_cob",
                    ["kd_tty_npps"]       = "src.kd_tty_npps",
                    ["nm_tipe_tty_npps"]       = "src.nm_tipe_tty_npps",
                    ["thn_tty_npps"]       = "src.thn_tty_npps",
                    ["npps_layer"]       = "src.npps_layer",
                    ["tgl_mul_ptg"]       = "src.tgl_mul_ptg",
                    ["tgl_akh_ptg"]       = "src.tgl_akh_ptg",
                    ["nilai_bts_or"]       = "src.nilai_bts_or",
                    ["nilai_bts_tty"]       = "src.nilai_bts_tty",
                    ["pst_adj_onrpi"]       = "src.pst_adj_onrpi",
                    ["ket_tty_npps"]       = "src.ket_tty_npps",
                },
                
                SearchableColumns = new List<string>
                {
                    "src.nm_cob",
                    "src.kd_tty_npps",
                    "src.nm_tipe_tty_npps",
                    "src.thn_tty_npps",
                    "src.npps_layer",
                    "src.tgl_mul_ptg",
                    "src.tgl_akh_ptg",
                    "src.nilai_bts_or",
                    "src.nilai_bts_tty",
                    "src.pst_adj_onrpi",
                    "src.ket_tty_npps"
                }
            };
        }
    }
}