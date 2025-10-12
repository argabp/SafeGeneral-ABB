using System;

namespace ABB.Domain.Entities
{
    public class InquiryObyekCIS
    {
        public string kd_cb { get; set; }

        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public string kd_thn { get; set; }

        public string no_pol { get; set; }

        public Int16 no_updt { get; set; }

        public Int16 no_rsk { get; set; }

        public string kd_endt { get; set; }

        public Int16 no_oby { get; set; }

        public DateTime tgl_oby { get; set; }

        public decimal nilai_saldo { get; set; }

        public string? no_pol_ttg { get; set; }
    }
}