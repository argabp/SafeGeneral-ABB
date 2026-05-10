using System.Collections.Generic;
using ABB.Application.Common.Grids.Models;

namespace ABB.Application.NotaFakultatifKeluars.Configs
{
    public static class DetailNotaFakultatifKeluarConfig
    {
        public static GridConfig Create()
        {
            return new GridConfig
            {
                FromSql = @"
                            FROM (
                                SELECT 
                                     RTRIM(p.kd_cb) + '.' + RTRIM(p.jns_tr) + '.' + 
                                     RTRIM(p.jns_nt_msk) + '.' + RTRIM(p.kd_thn) + '.' +  RTRIM(p.kd_bln) + '.' + 
                                     RTRIM(p.no_nt_msk) + '.' + RTRIM(p.jns_nt_kel) + '.' +  RTRIM(p.no_nt_kel) + CAST(p.no_ang AS VARCHAR) as id,
                                    p.*
                                FROM ri04ed01 p
                            ) src
                            ",
                
                BaseWhere = "(src.kd_cb = @kd_cb AND src.jns_tr = @jns_tr AND src.jns_nt_msk = @jns_nt_msk" +
                            " AND src.kd_thn = @kd_thn AND src.kd_bln = @kd_bln AND src.no_nt_msk = @no_nt_msk" +
                            " AND src.jns_nt_kel = @jns_nt_kel AND src.no_nt_kel = @no_nt_kel)",


                ColumnMap = new Dictionary<string, string>
                {
                    ["no_ang"]       = "src.no_ang",
                    ["tgl_ang"]      = "src.tgl_ang",
                    ["tgl_jth_tempo"]     = "src.tgl_jth_tempo",
                    ["pst_ang"]     = "src.pst_ang",
                    ["nilai_ang"]     = "src.nilai_ang"
                },
                
                SearchableColumns = new List<string>
                {
                    "src.no_ang",
                    "src.tgl_ang",
                    "src.tgl_jth_tempo",
                    "src.pst_ang",
                    "src.nilai_ang"
                }
            };
        }
    }
}