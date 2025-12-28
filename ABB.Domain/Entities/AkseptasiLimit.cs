using System;

namespace ABB.Domain.Entities
{
    public class AkseptasiLimit
    {
        public string kd_cb { get; set; }

        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public string kd_thn { get; set; }

        public string no_aks { get; set; }

        public Int16 no_urut { get; set; }

        public decimal pst_mul { get; set; }

        public decimal pst_akh { get; set; }

        public decimal pst_koas { get; set; }

        public decimal nilai_kapasitas_tty { get; set; }

        public decimal nilai_limit_tsi { get; set; }

        public decimal pst_share_max { get; set; }
    }
}