using System.Collections.Generic;
using ABB.Application.Common.Grids.Models;

namespace ABB.Application.CancelPostingNotaKlaimReasuransis.Configs
{
    public static class CancelPostingNotaKlaimReasuransiConfig
    {
        public static GridConfig Create()
        {
            return new GridConfig
            {
                FromSql = @"
                            FROM (
                                SELECT 
                                    p.*,
                                    cb.nm_cb,
                                    cob.nm_cob,
                                    scob.nm_scob,
                                     'K.' + RTRIM(p.kd_cb) + '.' + RTRIM(p.kd_scob) + '.' + 
                                     RTRIM(p.kd_thn) + '.' + RTRIM(p.no_kl) + CAST(p.no_mts AS VARCHAR) as nomor_register,
                                     RTRIM(p.kd_cb) + '-' + RTRIM(p.kd_cob) + '-' + RTRIM(p.kd_scob) + '-' + 
                                        RTRIM(p.kd_thn) + '-' + RTRIM(p.no_kl) + '-' + CAST(p.no_mts AS VARCHAR) + '-' + 
                                        CAST(p.st_tipe_dla AS VARCHAR) AS Id
                                 FROM v_cl10r p
                                    INNER JOIN rf01 cb ON p.kd_cb = cb.kd_cb
                                    INNER JOIN rf04 cob ON p.kd_cob = cob.kd_cob
                                    INNER JOIN rf05 scob ON p.kd_cob = scob.kd_cob 
                                                        AND p.kd_scob = scob.kd_scob
                            ) src
                            ",

                BaseWhere = @"
                            (src.flag_cancel = 'N' AND src.flag_posting = 'Y')
                            ",

                ColumnMap = new Dictionary<string, string>
                {
                    ["nomor_register"] = "src.nomor_register",
                    ["nm_cb"] = "src.nm_cb",
                    ["nm_cob"] = "src.nm_cob",
                    ["nm_scob"] = "src.nm_scob",
                    ["nm_ttg"] = "src.nm_ttg"
                },

                SearchableColumns = new List<string>
                {
                    "src.nm_ttg",
                    "src.nm_scob",
                    "src.nm_cob",
                    "src.nm_cb",
                    "src.nomor_register"
                }
            };
        }
    }
}