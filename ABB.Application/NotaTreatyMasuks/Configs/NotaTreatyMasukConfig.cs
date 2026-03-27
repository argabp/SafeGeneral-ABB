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
                                    cb.nm_cb,
                                    s.nm_jns_sor,
                                    m.nm_mtu
                                FROM ri02i p
                                INNER JOIN rf01 cb ON p.kd_cb = cb.kd_cb
                                INNER JOIN rf18 s ON p.kd_jns_sor = s.kd_jns_sor
                                INNER JOIN rf06 m ON p.kd_mtu = m.kd_mtu
                            ) src
                            ",
                
                BaseWhere = "(src.kd_jns_sor <> 'XOL')",


                ColumnMap = new Dictionary<string, string>
                {
                    ["nm_cb"]       = "src.nm_cb",
                    ["nm_jns_sor"]       = "src.nm_jns_sor",
                    ["nm_mtu"]       = "src.nm_mtu",
                    ["nomor_transaksi"]       = "src.nomor_transaksi",
                    ["kd_tty_msk"]       = "src.kd_tty_msk"
                },
                
                SearchableColumns = new List<string>
                {
                    "src.nm_cb",
                    "src.nm_jns_sor",
                    "src.nm_mtu",
                    "src.nomor_transaksi",
                    "src.kd_tty_msk"
                }
            };
        }
    }
}