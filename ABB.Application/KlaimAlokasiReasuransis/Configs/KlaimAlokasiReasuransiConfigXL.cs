using System.Collections.Generic;
using ABB.Application.Common.Grids.Models;

namespace ABB.Application.KlaimAlokasiReasuransis.Configs
{
    public static class KlaimAlokasiReasuransiXLConfig
    {
        public static GridConfig Create()
        {
            return new GridConfig
            {
                FromSql = @"FROM (
                                SELECT 
                                    -- Unique ID generated from the 8 Primary Key columns of cl07
                                    RTRIM(c7.kd_cb) + '-' + 
                                    RTRIM(c7.kd_cob) + '-' + 
                                    RTRIM(c7.kd_scob) + '-' + 
                                    RTRIM(c7.kd_thn) + '-' + 
                                    RTRIM(c7.no_kl) + '-' + 
                                    CAST(c7.no_mts AS VARCHAR) + '-' + 
                                    RTRIM(c7.kd_jns_sor) + '-' + 
                                    RTRIM(c7.kd_kontr) AS Id,
                                    
                                    -- All columns from cl07
                                    c7.kd_cb,
                                    c7.kd_cob,
                                    c7.kd_scob,
                                    c7.kd_thn,
                                    c7.no_kl,
                                    c7.no_mts,
                                    c7.kd_jns_sor,
                                    -- Name from rf18
                                    jns.nm_jns_sor AS nm_jns_sor,
                                    c7.kd_kontr,
                                    -- Name from v_ri02t
                                    kontr.nm_tty_npps AS nm_kontr,
                                    c7.nilai_kl,
                                    c7.nilai_reinst
                                FROM dbo.cl07 c7
                                -- Join for nm_kd_jns_sor
                                LEFT JOIN rf18 jns ON c7.kd_jns_sor = jns.kd_jns_sor
                                -- Join for nm_kd_kontr from v_ri02t
                                LEFT JOIN v_ri02t kontr ON c7.kd_kontr = kontr.kd_tty_npps
                            ) src
                            ",
                
                BaseWhere = @"src.kd_cb = @kd_cb AND src.kd_cob = @kd_cob 
                            AND src.kd_scob = @kd_scob AND src.kd_thn = @kd_thn 
                            AND src.no_kl = @no_kl AND src.no_mts = @no_mts",

                ColumnMap = new Dictionary<string, string>
                {
                    ["Id"]             = "Id",
                    ["kd_cb"]          = "src.kd_cb",
                    ["kd_cob"]         = "src.kd_cob",
                    ["kd_scob"]        = "src.kd_scob",
                    ["kd_thn"]         = "src.kd_thn",
                    ["no_kl"]          = "src.no_kl",
                    ["no_mts"]         = "src.no_mts",
                    ["kd_jns_sor"]     = "src.kd_jns_sor",
                    ["kd_kontr"]       = "src.kd_kontr",
                    ["nilai_kl"]       = "src.nilai_kl",
                    ["nilai_reinst"]   = "src.nilai_reinst"
                },

                SearchableColumns = new List<string>
                {
                    "src.kd_jns_sor",
                    "src.kd_kontr",
                    "src.nilai_kl",
                    "src.nilai_reinst"
                }
            };
        }
    }
}