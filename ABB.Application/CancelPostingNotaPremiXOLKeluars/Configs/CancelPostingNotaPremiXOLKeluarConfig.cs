using System.Collections.Generic;
using ABB.Application.Common.Grids.Models;

namespace ABB.Application.CancelPostingNotaPremiXOLKeluars.Configs
{
    public static class CancelPostingNotaPremiXOLKeluarConfig
    {
        public static GridConfig Create()
        {
            return new GridConfig
            {
                FromSql = @"
                            FROM (
                                SELECT 
                                    p.*,
                                     RTRIM(p.jns_sb_nt) + '.' + RTRIM(p.kd_cb) + '.' + RTRIM(p.jns_tr) + '.' + 
                                     RTRIM(p.jns_nt_msk) + '.' + RTRIM(p.kd_thn) + '.' +  RTRIM(p.kd_bln) + '.' + 
                                     RTRIM(p.no_nt_msk) + '.' + RTRIM(p.jns_nt_kel) + '.' +  RTRIM(p.no_nt_kel) as id,
                                     RTRIM(p.kd_cb) + '.' + RTRIM(p.jns_tr) + '.' + 
                                     RTRIM(p.jns_nt_msk) + '.' + RTRIM(p.kd_thn) + '.' +  RTRIM(p.kd_bln) + '.' + 
                                     RTRIM(p.no_nt_msk) + '.' + RTRIM(p.jns_nt_kel) + '.' +  RTRIM(p.no_nt_kel) as nomor_nota
                                 FROM fn05 p
                            ) src
                            ",

                BaseWhere = @"
								(src.flag_posting = 'Y') AND
								(( src.jns_tr = 'P' AND 
								  src.jns_nt_msk = '0' AND
								  src.jns_nt_kel = 'P') OR
								( src.jns_tr = 'P' AND 
								  src.jns_nt_msk = 'P' AND
								  src.jns_nt_kel = '0') OR			
								( src.jns_tr = 'K' AND 
								  src.jns_nt_msk = 'N' AND
								  src.jns_nt_kel = '0') OR
								( src.jns_tr = 'P' AND 
								  src.jns_nt_msk = '0' AND
								  src.jns_nt_kel = 'N') OR
								( src.jns_tr = 'P' AND 
								  src.jns_nt_msk = 'N' AND
								  src.jns_nt_kel = '0')) AND 
						         ( src.jns_nt_kel <> 'C')
                            ",

                ColumnMap = new Dictionary<string, string>
                {
                    ["nomor_nota"] = "src.nomor_nota",
                    ["nm_ttj"] = "src.nm_ttj"
                },

                SearchableColumns = new List<string>
                {
                    "src.nomor_nota",
                    "src.nm_ttj"
                }
            };
        }
    }
}