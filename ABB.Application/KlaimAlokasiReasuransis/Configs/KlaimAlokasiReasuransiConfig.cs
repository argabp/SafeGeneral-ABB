using System.Collections.Generic;
using ABB.Application.Common.Grids.Models;

namespace ABB.Application.KlaimAlokasiReasuransis.Configs
{
    public static class KlaimAlokasiReasuransiConfig
    {
        public static GridConfig Create()
        {
            return new GridConfig
            {
                FromSql = @"
                            FROM dbo.cl06 c6
                                LEFT JOIN rf18 jns ON RTRIM(c6.kd_jns_sor) = RTRIM(jns.kd_jns_sor)

                                -- Lookup Rekanan
                                LEFT JOIN rf03 rk ON RTRIM(c6.kd_rk_sor) = RTRIM(rk.kd_rk) 
                                                AND rk.kd_grp_rk = '5'
                                                AND RTRIM(rk.kd_cb) = RTRIM(c6.kd_cb)

                                -- Lookup Treaty (WITH FIX FOR DUPLICATE 17045)
                                LEFT JOIN ri01t tty ON RTRIM(c6.kd_rk_sor) = RTRIM(tty.kd_tty_pps)
                                                AND RTRIM(c6.kd_jns_sor) = RTRIM(tty.kd_jns_sor)
                                                AND RTRIM(c6.kd_cob) = RTRIM(tty.kd_cob) -- Added this to distinguish between Surety (B) and Aneka (V)

                                CROSS APPLY (
                                    SELECT 
                                        c6.kd_cb, c6.kd_cob, c6.kd_scob, c6.kd_thn, c6.no_kl, c6.no_mts,
                                        c6.kd_jns_sor, c6.kd_grp_sor, c6.kd_rk_sor,
                                        c6.pst_share, c6.nilai_kl, c6.flag_cash_call, c6.flag_nota, c6.flag_nt,

                                        RTRIM(c6.kd_cb) + '-' + RTRIM(c6.kd_cob) + '-' + RTRIM(c6.kd_scob) + '-' + 
                                        RTRIM(c6.kd_thn) + '-' + RTRIM(c6.no_kl) + '-' + CAST(c6.no_mts AS VARCHAR) + '-' + 
                                        RTRIM(c6.kd_jns_sor) + '-' + RTRIM(c6.kd_grp_sor) + '-' + RTRIM(c6.kd_rk_sor) AS Id,
                                        
                                        RTRIM(jns.nm_jns_sor) as nm_jns_sor,
                                        CASE 
                                            WHEN tty.nm_tty_pps IS NOT NULL THEN RTRIM(tty.nm_tty_pps)
                                            WHEN rk.nm_rk IS NOT NULL THEN RTRIM(rk.nm_rk)
                                            ELSE ''
                                        END AS nm_rk_sor
                                ) as src
                            ",
                
               BaseWhere = @"src.kd_cb = @kd_cb AND src.kd_cob = @kd_cob 
                            AND src.kd_scob = @kd_scob AND src.kd_thn = @kd_thn 
                            AND src.no_kl = @no_kl AND src.no_mts = @no_mts",

                ColumnMap = new Dictionary<string, string>
                {
                    ["Id"]             = "Id", // The composite key column
                    ["kd_cb"]          = "src.kd_cb",
                    ["kd_cob"]         = "src.kd_cob",
                    ["kd_scob"]        = "src.kd_scob",
                    ["kd_thn"]         = "src.kd_thn",
                    ["no_kl"]          = "src.no_kl",
                    ["no_mts"]         = "src.no_mts",
                    ["kd_jns_sor"]     = "src.kd_jns_sor",
                    ["kd_grp_sor"]     = "src.kd_grp_sor",
                    ["kd_rk_sor"]      = "src.kd_rk_sor",
                    ["pst_share"]      = "src.pst_share",
                    ["nilai_kl"]       = "src.nilai_kl",
                    ["flag_cash_call"] = "src.flag_cash_call",
                    ["flag_nota"]      = "src.flag_nota",
                    ["flag_nt"]        = "src.flag_nt"
                },

                SearchableColumns = new List<string>
                {
                    "src.kd_jns_sor",
                    "src.kd_grp_sor",
                    "src.kd_rk_sor",
                    "src.flag_cash_call",
                    "src.flag_nota",
                    "src.flag_nt"
                }
            };
        }
    }
}