using System.Collections.Generic;
using ABB.Application.Common.Grids.Models;

namespace ABB.Application.ProsesPremiXOLKeluars.Configs
{
    public static class ProsesPremiXOLKeluarConfig
    {
        public static GridConfig Create()
        {
            return new GridConfig
            {
                FromSql = @"
                            FROM (
                                SELECT 
                                    RTRIM(p.kd_cb) + RTRIM(p.kd_thn) + RTRIM(p.kd_bln) + RTRIM(p.kd_jns_sor) + RTRIM(p.kd_tty_npps) + RTRIM(p.kd_mtu) + RTRIM(p.no_tr) AS Id,
                                    RTRIM(p.kd_thn) + '.' + RTRIM(p.kd_bln) + '.' + RTRIM(p.no_tr) AS nomor_transaksi,
                                    p.*,
                                    s.nm_jns_sor,
                                    m.nm_mtu,
                                    cb.nm_cb
                                FROM ri03t p
                                INNER JOIN rf01 cb ON p.kd_cb = cb.kd_cb
                                INNER JOIN rf18 s ON p.kd_jns_sor = s.kd_jns_sor
                                INNER JOIN rf06 m ON p.kd_mtu = m.kd_mtu
                            ) src
                            ",
                
                BaseWhere = "",


                ColumnMap = new Dictionary<string, string>
                {
                    ["no_ref"]       = "src.no_ref",
                    ["nm_jns_sor"]       = "src.nm_jns_sor",
                    ["tgl_closing"]       = "src.tgl_closing",
                    ["flag_closing"]       = "src.flag_closing",
                    ["kd_tty_npps"]       = "src.kd_tty_npps"
                },
                
                SearchableColumns = new List<string>
                {
                    "src.no_ref",
                    "src.nm_jns_sor",
                    "src.tgl_closing",
                    "src.flag_closing",
                    "src.kd_tty_npps"
                }
            };
        }
    }
}