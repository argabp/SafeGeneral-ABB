using System;

namespace ABB.Domain.Entities
{
    public class InquiryOtherMotor
    {
        public string kd_cb { get; set; }

        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public string kd_thn { get; set; }

        public string no_pol { get; set; }

        public Int16 no_updt { get; set; }

        public Int16 no_rsk { get; set; }

        public string kd_endt { get; set; }

        public string? no_pls { get; set; }

        public string? grp_jns_kend { get; set; }

        public string kd_jns_kend { get; set; }

        public string? grp_merk_kend { get; set; }

        public string kd_merk_kend { get; set; }

        public string? tipe_kend { get; set; }

        public string? warna_kend { get; set; }

        public string no_rangka { get; set; }

        public string no_msn { get; set; }

        public decimal? thn_buat { get; set; }

        public string? kpsts_msn { get; set; }

        public byte? jml_tempat_ddk { get; set; }

        public string kd_jns_ptg { get; set; }

        public string kd_guna { get; set; }

        public string kd_utk { get; set; }

        public decimal nilai_casco { get; set; }

        public decimal nilai_tjh { get; set; }
        
        public decimal nilai_tjp { get; set; }

        public decimal nilai_pap { get; set; }
        
        public decimal nilai_pad { get; set; }

        public decimal pst_rate_prm { get; set; }

        public byte stn_rate_prm { get; set; }

        public decimal pst_rate_hh { get; set; }

        public byte stn_rate_hh { get; set; }

        public decimal nilai_rsk_sendiri { get; set; }

        public decimal nilai_prm_casco { get; set; }

        public decimal nilai_prm_tjh { get; set; }

        public decimal nilai_prm_tjp { get; set; }

        public decimal nilai_prm_pap { get; set; }

        public decimal nilai_prm_pad { get; set; }

        public decimal nilai_prm_hh { get; set; }

        public DateTime? tgl_mul_ptg { get; set; }

        public DateTime? tgl_akh_ptg { get; set; }

        public string? no_pinj { get; set; }

        public decimal? nilai_bia_pol { get; set; }

        public decimal? nilai_bia_mat { get; set; }

        public string? nm_qq { get; set; }

        public string? almt_qq { get; set; }

        public string? kt_qq { get; set; }

        public decimal? nilai_pap_med { get; set; }
        
        public decimal? nilai_pad_med { get; set; }

        public decimal? nilai_prm_pap_med { get; set; }

        public decimal? nilai_prm_pad_med { get; set; }

        public decimal nilai_prm_aog { get; set; }

        public decimal pst_rate_aog { get; set; }

        public byte stn_rate_aog { get; set; }

        public string? no_pol_ttg { get; set; }

        public decimal? pst_rate_banjir { get; set; }

        public byte? stn_rate_banjir { get; set; }

        public decimal? nilai_prm_banjir { get; set; }

        public string? kd_wilayah { get; set; }

        public string validitas { get; set; }

        public byte? stn_rate_trs { get; set; }

        public decimal? pst_rate_trs { get; set; }

        public decimal? nilai_prm_trs { get; set; }

        public string? flag_hh { get; set; }
        
        public string? flag_aog { get; set; }
        
        public string? flag_banjir { get; set; }
        
        public string? flag_trs { get; set; }

        public byte? stn_rate_tjh { get; set; }

        public decimal? pst_rate_tjh { get; set; }

        public byte? stn_rate_tjp { get; set; }

        public decimal? pst_rate_tjp { get; set; }

        public byte? stn_rate_pap { get; set; }

        public decimal? pst_rate_pap { get; set; }

        public byte? stn_rate_pad { get; set; }

        public decimal? pst_rate_pad { get; set; }

        public byte? jml_pap { get; set; }
    }
}