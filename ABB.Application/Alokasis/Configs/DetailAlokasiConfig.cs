using System.Collections.Generic;
using ABB.Application.Common.Grids.Models;

namespace ABB.Application.Alokasis.Configs
{
    public static class DetailAlokasiConfig
    {
        public static GridConfig Create()
        {
            return new GridConfig
            {
                FromSql = @"FROM (SELECT p.* FROM ri02e p) src",
                
                BaseWhere = "(src.kd_cb = @kd_cb AND src.kd_cob = @kd_cob AND" +
                            " src.kd_scob = @kd_scob AND src.kd_thn = @kd_thn AND" +
                            " src.no_pol = @no_pol AND src.no_updt = @no_updt AND" +
                            " src.no_rsk = @no_rsk AND src.kd_endt = @kd_endt)",

                ColumnMap = new Dictionary<string, string>
                {
                    ["kd_jns_sor"]       = "src.kd_jns_sor",
                    ["kd_rk_sor"]      = "src.kd_rk_sor",
                    ["nilai_ttl_ptg_reas"]     = "src.nilai_ttl_ptg_reas",
                    ["pst_share"] = "src.pst_share",
                    ["nilai_prm_reas"] = "src.nilai_prm_reas",
                    ["pst_kms_reas"] = "src.pst_kms_reas",
                    ["nilai_kms_reas"] = "src.nilai_kms_reas",
                    ["pst_adj_reas"] = "src.pst_adj_reas",
                    ["stn_adj_reas"] = "src.stn_adj_reas",
                    ["nilai_adj_reas"] = "src.nilai_adj_reas"
                },
                
                SearchableColumns = new List<string>
                {
                    "src.kd_jns_sor",
                    "src.kd_rk_sor",
                    "src.nilai_ttl_ptg_reas",
                    "src.pst_share",
                    "src.nilai_prm_reas",
                    "src.pst_kms_reas",
                    "src.nilai_kms_reas",
                    "src.pst_adj_reas",
                    "src.stn_adj_reas",
                    "src.nilai_adj_reas"
                }
            };
        }
    }
}