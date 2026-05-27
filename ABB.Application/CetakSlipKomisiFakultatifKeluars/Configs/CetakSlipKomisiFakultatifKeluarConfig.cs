using System.Collections.Generic;
using ABB.Application.Common.Grids.Models;

namespace ABB.Application.CetakSlipKomisiFakultatifKeluars.Configs
{
    public static class CetakSlipKomisiFakultatifKeluarConfig
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
									r.nm_rk nm_ttj,
                                     RTRIM(p.kd_cb) + '.' + RTRIM(p.jns_tr) + '.' + 
                                     RTRIM(p.jns_nt_msk) + '.' + RTRIM(p.kd_thn) + '.' +  RTRIM(p.kd_bln) + '.' + 
                                     RTRIM(p.no_nt_msk) + '.' + RTRIM(p.jns_nt_kel) + '.' +  RTRIM(p.no_nt_kel) as id,
                                     RTRIM(p.kd_cb) + '.' + RTRIM(p.jns_tr) + '.' + 
                                     RTRIM(p.jns_nt_msk) + '.' + RTRIM(p.kd_thn) + '.' +  RTRIM(p.kd_bln) + '.' + 
                                     RTRIM(p.no_nt_msk) + '.' + RTRIM(p.jns_nt_kel) + '.' +  RTRIM(p.no_nt_kel) as nomor_nota,
									 p3.nm_ttg
                                 FROM ri04e p
                                    INNER JOIN rf01 cb ON p.kd_cb_pol = cb.kd_cb
                                    INNER JOIN rf04 cob ON p.kd_cob = cob.kd_cob
                                    INNER JOIN rf05 scob ON p.kd_cob = scob.kd_cob 
                                                        AND p.kd_scob = scob.kd_scob
                                INNER JOIN rf03 r 
                                    ON p.kd_rk_pas = r.kd_rk
                                        AND p.kd_grp_pas = r.kd_grp_rk
                                        AND p.kd_cb = r.kd_cb
								INNER JOIN ri01e p3
									ON p.kd_cb_pol = p3.kd_cb
										AND p.kd_cob=p3.kd_cob
										AND p.kd_scob=p3.kd_scob
										AND p.kd_thn =p3.kd_thn 
										AND p.no_pol=p3.no_pol 
										AND p.no_updt=p3.no_updt
										AND p.no_rsk=p3.no_rsk 
										AND p.kd_endt=p3.kd_endt
                            ) src
                            ",

                BaseWhere = @"
                            (src.flag_cancel = 'N')
                            ",

                ColumnMap = new Dictionary<string, string>
                {
	                ["nm_cb"]       = "src.nm_cb",
	                ["nm_cob"]      = "src.nm_cob",
	                ["nm_scob"]     = "src.nm_scob",
	                ["no_pol_lama"] = "src.no_pol_lama",
	                ["no_rsk"]     = "src.no_rsk",
	                ["nomor_nota"]     = "src.nomor_nota",
	                ["nm_ttg"] = "src.nm_ttg",
	                ["nm_ttj"] = "src.nm_ttj",
	                ["nilai_nt"] = "src.nilai_nt",
	                ["flag_posting"] = "src.flag_posting"
                },

                SearchableColumns = new List<string>
                {
	                "src.nm_cb",
	                "src.nm_cob",
	                "src.nm_scob",
	                "src.no_pol_lama",
	                "src.no_rsk",
	                "src.nomor_nota",
	                "src.nm_ttg",
	                "src.nm_ttj",
	                "src.nilai_nt",
	                "src.flag_posting"
                }
            };
        }
    }
}