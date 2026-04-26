using System.Collections.Generic;
using ABB.Application.Common.Grids.Models;

namespace ABB.Application.Alokasis.Configs
{
    public static class SORConfig
    {
        public static GridConfig Create()
        {
            return new GridConfig
            {
                FromSql = @"
                    FROM (  
                        SELECT DISTINCT TOP 100 
                            CAST(uw01e.kd_cb AS VARCHAR) + '-' + 
                                CAST(uw01e.kd_cob AS VARCHAR) + '-' + 
                                CAST(uw01e.kd_scob AS VARCHAR) + '-' + 
                                CAST(uw01e.kd_thn AS VARCHAR) + '-' + 
                                CAST(uw01e.no_pol AS VARCHAR) + '-' + 
                                CAST(uw01e.no_updt AS VARCHAR) AS Id,
                            uw01e.kd_cb,   
                             uw01e.kd_cob,   
                             uw01e.kd_scob,   
                             uw01e.kd_thn,   
                             uw01e.no_pol,   
                             uw01e.no_updt,   
                             uw01e.no_renew,   
                             uw01e.thn_uw,   
                             uw01e.no_endt,   
                             uw01e.nm_ttg,   
                             uw01e.tgl_mul_ptg,   
                             uw01e.tgl_akh_ptg,   
                             uw01e.kd_usr_input,   
                             uw01e.tgl_input,   
                             uw01e.tgl_closing,   
                             ri01e.tgl_closing tgl_closing_reas,  
                             ri01e.flag_closing,   
                            cb.nm_cb,
                            cob.nm_cob,
                            scob.nm_scob,
                            SUBSTRING(uw01e.no_pol_ttg, 1, 2) + '.' +
                            SUBSTRING(uw01e.no_pol_ttg, 3, 3) + '.' +
                            SUBSTRING(uw01e.no_pol_ttg, 6, 2) + '.' +
                            SUBSTRING(uw01e.no_pol_ttg, 8, 4) + '.' +
                            SUBSTRING(uw01e.no_pol_ttg, 12, 4) + '-' +
                            SUBSTRING(uw01e.no_pol_ttg, 16, LEN(uw01e.no_pol_ttg)) no_pol_ttg_masked
                            FROM uw01e 
                            LEFT JOIN ri01e ON 
                                uw01e.kd_cb = ri01e.kd_cb AND 
                                uw01e.kd_cob = ri01e.kd_cob AND 
                                uw01e.kd_scob = ri01e.kd_scob AND 
                                uw01e.kd_thn = ri01e.kd_thn AND 
                                uw01e.no_pol = ri01e.no_pol AND 
                                uw01e.no_updt = ri01e.no_updt
                            LEFT JOIN rf01 cb ON uw01e.kd_cb = cb.kd_cb
                            LEFT JOIN rf04 cob ON uw01e.kd_cob = cob.kd_cob
                        LEFT JOIN rf05 scob ON uw01e.kd_cob = scob.kd_cob 
                                           AND uw01e.kd_scob = scob.kd_scob 
                    ) as src",

                BaseWhere = @"",

                ColumnMap = new Dictionary<string, string>
                {
                    ["nm_cb"] = "src.nm_cb",
                    ["nm_cob"] = "src.nm_cob",
                    ["nm_scob"] = "src.nm_scob",
                    ["tgl_closing"] = "src.tgl_closing",
                    ["tgl_closing_reas"] = "src.tgl_closing_reas",
                    ["flag_closing"] = "src.flag_closing",
                    ["tgl_mul_ptg"] = "src.tgl_mul_ptg",
                    ["tgl_akh_ptg"] = "src.tgl_akh_ptg",
                    ["no_pol_ttg_masked"] = "src.no_pol_ttg_masked",
                    ["nm_ttg"] = "src.nm_ttg"
                },

                SearchableColumns = new List<string>
                {
                    "src.nm_cb",
                    "src.nm_cob",
                    "src.nm_scob",
                    "src.nm_ttg",
                    "src.tgl_closing",
                    "src.tgl_closing_reas",
                    "src.flag_closing",
                    "src.tgl_mul_ptg",
                    "src.tgl_akh_ptg",
                    "src.no_pol_ttg_masked"
                }
            };
        }
    }
}