using System;

namespace ABB.Domain.Entities
{
    public class PesertaSirama
    {
        public string kd_cb { get; set; }

        public string kd_product { get; set; }

        public string kd_thn { get; set; }

        public string kd_rk { get; set; }

        public string no_sppa { get; set; }

        public Int16 no_updt { get; set; }

        public string kd_tujuan_pinjmn { get; set; }

        public string jns_pembiayaan { get; set; }

        public string jns_jaminan { get; set; }

        public string kd_mtu { get; set; }

        public decimal nilai_ptg { get; set; }

        public DateTime tgl_pk { get; set; }

        public DateTime tgl_mulai { get; set; }

        public DateTime tgl_akh { get; set; }

        public string jns_program { get; set; }

        public string kd_kategori { get; set; }

        public decimal pst_rate { get; set; }

        public Int16 stn_rate { get; set; }

        public decimal nilai_prm { get; set; }

        public decimal loading_prm { get; set; }

        public decimal total_prm { get; set; }

        public int? periode_asuransi { get; set; }
    }
}