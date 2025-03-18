using System;

namespace ABB.Domain.Entities
{
    public class PesertaSiramaBackup
    {
        public string kd_cb { get; set; }

        public string kd_product { get; set; }

        public string kd_thn { get; set; }

        public string kd_rk { get; set; }

        public string no_sppa { get; set; }

        public Int16 no_updt { get; set; }

        public string kd_kategori { get; set; }

        public decimal pst_rate { get; set; }

        public Int16 stn_rate { get; set; }

        public decimal nilai_prm { get; set; }

        public decimal pst_loading_prm { get; set; }

        public decimal loading_prm { get; set; }

        public decimal total_prm { get; set; }

        public decimal pst_fac { get; set; }

        public decimal fac_prm { get; set; }
    }
}