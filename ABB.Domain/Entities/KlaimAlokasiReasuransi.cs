using System;
using System.Collections.Generic;
using System.Text;

namespace ABB.Domain.Entities
{
    public class KlaimAlokasiReasuransi
    {
        // Primary Key / Foreign Key Fields
        public string kd_cb { get; set; }
        public string kd_cob { get; set; }
        public string kd_scob { get; set; }
        public string kd_thn { get; set; }
        public string no_kl { get; set; }
        public short no_mts { get; set; }
        public string kd_jns_sor { get; set; }
        public string kd_grp_sor { get; set; }
        public string kd_rk_sor { get; set; }

        // Data Fields
        public decimal pst_share { get; set; }
        public decimal nilai_kl { get; set; }
        public string flag_cash_call { get; set; }
        public string flag_nota { get; set; }
        public string? flag_nt { get; set; } // Nullable as per schema
    }
}
