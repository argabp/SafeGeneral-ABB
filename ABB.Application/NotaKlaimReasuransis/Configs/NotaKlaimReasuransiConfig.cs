using System.Collections.Generic;
using ABB.Application.Common.Grids.Models;

namespace ABB.Application.NotaKlaimReasuransis.Configs
{
    public static class NotaKlaimReasuransiConfig
    {
        public static GridConfig Create()
        {
            return new GridConfig
            {
                FromSql = @"
                            FROM (
                                SELECT 
                                    p.*,
                                    cob.nm_cob,
                                     RTRIM(p.kd_cb) + '.' + RTRIM(p.jns_tr) + '.' + 
                                     RTRIM(p.jns_nt_msk) + '.' + RTRIM(p.kd_thn) + '.' +  RTRIM(p.kd_bln) + '.' + 
                                     RTRIM(p.no_nt_msk) + '.' + RTRIM(p.jns_nt_kel) + '.' +  RTRIM(p.no_nt_kel) as nomor_nota,
                                     RTRIM(p.kd_cb) + '.' + RTRIM(p.kd_cob) + '.' + 
                                     RTRIM(p.kd_scob) + '.' + RTRIM(p.kd_thn) + '.' +  RTRIM(p.no_kl) + '.' + 
                                     RTRIM(p.no_mts) + '.' + RTRIM(p.no_dla) as nomor_dla
                                 FROM cl10r p
                                    INNER JOIN rf04 cob ON p.kd_cob = cob.kd_cob
                            ) src
                            ",

                BaseWhere = @"
                            (src.flag_cancel = 'N')
                            ",

                ColumnMap = new Dictionary<string, string>
                {
                    ["nomor_nota"] = "src.nomor_nota",
                    ["nomor_dla"] = "src.nomor_dla",
                    ["nm_cob"] = "src.nm_cob",
                    ["nm_ttj"] = "src.nm_ttj",
                    ["flag_posting"] = "src.flag_posting",
                    ["flag_cancel"] = "src.flag_cancel"
                },

                SearchableColumns = new List<string>
                {
                    "src.nomor_nota",
                    "src.nomor_dla",
                    "src.nm_cob",
                    "src.nm_ttj",
                    "src.flag_posting",
                    "src.flag_cancel"
                }
            };
        }
    }
}