using System;

namespace ABB.Domain.Entities
{
    public class PesertaStatusAll
    {
        public string kd_cb { get; set; }

        public string kd_product { get; set; }

        public string kd_thn { get; set; }

        public string kd_rk { get; set; }

        public string no_sppa { get; set; }

        public Int16 no_updt { get; set; }
        public Int16 no_urut { get; set; }

        public string kd_user { get; set; }

        public DateTime tgl_status { get; set; }
        
        public Int16 kd_status { get; set; }

        public string kd_user_status { get; set; }

        public string keterangan { get; set; }
    }
}