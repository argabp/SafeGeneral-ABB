using System;

namespace ABB.Application.MutasiKlaims.Queries
{
    public class MutasiKlaimDetailDto
    {
        public int Id { get; set; }
        
        public string kd_cb { get; set; }

        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public string kd_thn { get; set; }

        public string no_kl { get; set; }

        public Int16 no_mts { get; set; }

        public string tipe_mts { get; set; }

        public string nm_tipe_mts { get; set; }

        public string kd_mtu { get; set; }

        public string nm_kd_mtu { get; set; }
        public DateTime tgl_mts { get; set; }
        public DateTime? tgl_closing { get; set; }

        public string kd_usr_input { get; set; }

        public decimal nilai_ttl_kl { get; set; }

        public string flag_closing { get; set; }

        public string flag_pol_lama { get; set; }
    }
}