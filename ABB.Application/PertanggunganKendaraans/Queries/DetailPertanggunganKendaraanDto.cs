using System;

namespace ABB.Application.PertanggunganKendaraans.Queries
{
    public class DetailPertanggunganKendaraanDto
    {
        public string Id { get; set; }
        
        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public string kd_jns_ptg { get; set; }

        public Int16 no_urut { get; set; }

        public decimal? nilai_tsi_tjh_mul { get; set; }

        public decimal? nilai_tsi_tjh_akh { get; set; }

        public decimal? pst_rate_tjh { get; set; }

        public Int16? stn_rate_tjh { get; set; }
        public string text_stn_rate_tjh { get; set; }

        public decimal? nilai_prm_tjh { get; set; }

        public decimal? nilai_tsi_tjp { get; set; }

        public decimal? nilai_prm_tjp { get; set; }

        public decimal? pst_rate_pad { get; set; }

        public decimal? pst_rate_pap { get; set; }
    }
}