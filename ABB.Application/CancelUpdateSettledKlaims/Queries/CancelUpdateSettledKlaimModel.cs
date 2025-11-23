using System;

namespace ABB.Application.CancelUpdateSettledKlaims.Queries
{
    public class CancelUpdateSettledKlaimModel
    {
        public string kd_cb { get; set; }

        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public string kd_thn { get; set; }

        public string no_kl { get; set; }

        public DateTime tgl_updt { get; set; }
    }
}