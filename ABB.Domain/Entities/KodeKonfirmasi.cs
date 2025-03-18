using System;

namespace ABB.Domain.Entities
{
    public class KodeKonfirmasi
    {
        public string kd_cb { get; set; }

        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public string kd_thn { get; set; }

        public string no_aks { get; set; }

        public string kd_konfirm { get; set; }

        public DateTime tgl_input { get; set; }

        public string kd_usr_input { get; set; }
    }
}