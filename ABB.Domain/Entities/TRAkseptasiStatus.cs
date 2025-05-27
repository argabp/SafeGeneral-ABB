using System;

namespace ABB.Domain.Entities
{
    public class TRAkseptasiStatus
    {
        public string kd_cb { get; set; }
        
        public string kd_cob { get; set; }
        
        public string kd_scob { get; set; }

        public string kd_thn { get; set; }

        public string no_aks { get; set; }

        public Int16 no_urut { get; set; }

        public string kd_user { get; set; }

        public string kd_user_sign { get; set; }

        public Int16 kd_status { get; set; }

        public DateTime tgl_status { get; set; }

        public Int16 sla_awal { get; set; }

        public DateTime tgl_batas { get; set; }

        public DateTime tgl_update { get; set; }

        public Int16 sla_update { get; set; }

        public Int16 reminder { get; set; }

        public string flag_update { get; set; }

    }
}