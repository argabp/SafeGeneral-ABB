using System;

namespace ABB.Application.KapasitasCabangs.Queries
{
    public class KapasitasCabangDto
    {
        public string Id { get; set; }
        
        public string kd_cb { get; set; }

        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public string nm_cb { get; set; }

        public string nm_cob { get; set; }

        public string nm_scob { get; set; }

        public int thn { get; set; }

        public decimal nilai_kapasitas { get; set; }

        public DateTime tgl_input { get; set; }

        public string kd_usr_input { get; set; }

        public decimal? nilai_kl { get; set; }
    }
}