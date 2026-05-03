using System.Collections.Generic;
using ABB.Application.Common.Grids.Models;

namespace ABB.Application.NotaTreatyMasuks.Configs
{
    public static class NotaTreatyMasukConfig
    {
        public static GridConfig Create()
        {
            return new GridConfig
            {
                FromSql = @"
                            FROM (
                                SELECT 
                                    RTRIM(p.kd_cb) + RTRIM(p.kd_thn) + RTRIM(p.kd_bln) + RTRIM(p.kd_jns_sor) + RTRIM(p.kd_tty_msk) + RTRIM(p.kd_mtu) + RTRIM(p.no_tr) AS Id,
                                    RTRIM(p.kd_thn) + '.' + RTRIM(p.kd_bln) + '.' + RTRIM(p.no_tr) AS nomor_transaksi,
                                    p.*,
                                    s.nm_jns_sor
                                FROM ri02i p
                                INNER JOIN rf18 s ON p.kd_jns_sor = s.kd_jns_sor
                            ) src
                            ",
                
                BaseWhere = "(src.kd_jns_sor <> 'XOL')",


                ColumnMap = new Dictionary<string, string>
                {
                    ["ket_tr"]       = "src.ket_tr",
                    ["nm_jns_sor"]       = "src.nm_jns_sor",
                    ["flag_closing"]       = "src.flag_closing",
                    ["tgl_closing"]       = "src.tgl_closing",
                    ["kd_tty_msk"]       = "src.kd_tty_msk"
                },
                
                SearchableColumns = new List<string>
                {
                    "src.ket_tr",
                    "src.nm_jns_sor",
                    "src.flag_closing",
                    "src.tgl_closing",
                    "src.kd_tty_msk"
                }
            };
        }
    }
}