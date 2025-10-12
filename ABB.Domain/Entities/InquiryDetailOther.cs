using System;

namespace ABB.Domain.Entities
{
    public class InquiryDetailOther
    {
        public string kd_cb { get; set; }

        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public string kd_thn { get; set; }

        public string no_pol { get; set; }

        public Int16 no_updt { get; set; }

        public Int16 no_rsk { get; set; }

        public string kd_endt { get; set; }

        public decimal thn_ptg_kend { get; set; }

        public decimal nilai_casco { get; set; }

        public decimal nilai_tjh { get; set; }
        
        public decimal nilai_tjp { get; set; }

        public decimal nilai_pap { get; set; }

        public decimal nilai_pad { get; set; }

        public decimal nilai_rsk_sendiri { get; set; }

        public decimal nilai_prm_casco { get; set; }

        public decimal nilai_prm_tjh { get; set; }
        
        public decimal nilai_prm_tjp { get; set; }

        public decimal nilai_prm_pap { get; set; }

        public decimal nilai_prm_pad { get; set; }

        public decimal nilai_prm_hh { get; set; }

        public string? st_deffered { get; set; }

        public decimal? nilai_pap_med { get; set; }

        public decimal? nilai_pad_med { get; set; }

        public decimal? nilai_prm_pap_med { get; set; }

        public decimal? nilai_prm_pad_med { get; set; }

        public decimal? nilai_prm_aog { get; set; }

        public decimal? kd_jns_ptg_thn { get; set; }

        public string? no_pol_ttg { get; set; }

        public decimal? pst_rate_banjir { get; set; }

        public byte? stn_rate_banjir { get; set; }

        public decimal? nilai_prm_banjir { get; set; }

        public decimal? nilai_trs { get; set; }

        public decimal? nilai_prm_trs { get; set; }
    }
}