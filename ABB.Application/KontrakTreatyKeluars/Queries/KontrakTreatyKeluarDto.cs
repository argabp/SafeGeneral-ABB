using System;

namespace ABB.Application.KontrakTreatyKeluars.Queries
{
    public class KontrakTreatyKeluarDto
    {
        public string Id { get; set; }
        public string kd_cb { get; set; }

        public string nm_cb { get; set; }

        public string kd_jns_sor { get; set; }

        public string nm_jns_sor { get; set; }

        public string kd_tty_pps { get; set; }

        public string kd_cob { get; set; }

        public decimal thn_tty_pps { get; set; }
        
        public string nm_tty_pps { get; set; }
        
        public string? nm_jns_ptg { get; set; }

        public DateTime tgl_mul_ptg { get; set; }

        public DateTime tgl_akh_ptg { get; set; }

        public string frek_lap { get; set; }

        public decimal nilai_bts_cash_call { get; set; }

        public decimal nilai_bts_cash_call_idr { get; set; }

        public decimal nilai_bts_or { get; set; }
        
        public decimal nilai_bts_or_idr { get; set; }

        public decimal nilai_bts_tty { get; set; }

        public decimal nilai_bts_tty_idr { get; set; }

        public decimal pst_kms_reas { get; set; }

        public string? ket_tty_pps { get; set; }

        public decimal? pst_share_reas { get; set; }

        public decimal? pst_hf { get; set; }

        public decimal? nilai_kurs { get; set; }

        public decimal nilai_bts_cash_pla { get; set; }

        public decimal nilai_bts_cash_pla_idr { get; set; }

        public decimal? pst_profit_comm { get; set; }

        public string? faktor_sor { get; set; }
        
        public string? st_koas { get; set; }
    }
}