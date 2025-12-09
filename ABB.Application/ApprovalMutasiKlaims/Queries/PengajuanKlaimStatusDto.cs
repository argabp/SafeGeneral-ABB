using System;

namespace ABB.Application.ApprovalMutasiKlaims.Queries
{
    public class PengajuanKlaimStatusDto
    {
        public string Id { get; set; }

        public string kd_cb { get; set; }

        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public string kd_thn { get; set; }

        public string no_kl { get; set; }
        
        public Int16 no_mts { get; set; }

        public Int16 no_urut { get; set; }

        public DateTime tgl_status { get; set; }

        public string nm_status { get; set; }

        public string ket_status { get; set; }
        
        public string nm_user_sign { get; set; }
        
        public DateTime tgl_batas { get; set; }

        public int reminder { get; set; }

        public string nm_user { get; set; }
    }
}