using System;

namespace ABB.Domain.Entities
{
    public class MutasiKlaimAlokasi
    {
        public string kd_cb { get; set; }

        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public string kd_thn { get; set; }

        public string no_kl { get; set; }

        public Int16 no_mts { get; set; }

        public string kd_grp_pas { get; set; }
        
        public string kd_rk_pas { get; set; }

        public decimal pst_share { get; set; }
        
        public decimal nilai_kl { get; set; }
    }
}