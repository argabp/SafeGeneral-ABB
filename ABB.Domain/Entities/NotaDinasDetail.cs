using System;

namespace ABB.Domain.Entities
{
    public class NotaDinasDetail
    {
        public int id_nds { get; set; }

        public string kd_cb { get; set; }

        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public string kd_thn { get; set; }

        public string no_pol { get; set; }

        public int no_updt { get; set; }

        public int no_rsk { get; set; }

        public string kd_endt { get; set; }

        public int no_updt_reas { get; set; }

        public decimal prm_bruto { get; set; }

        public decimal kms { get; set; }

        public decimal pph { get; set; }

        public decimal pst_kms { get; set; }

        public decimal pst_pph { get; set; }
    }
}