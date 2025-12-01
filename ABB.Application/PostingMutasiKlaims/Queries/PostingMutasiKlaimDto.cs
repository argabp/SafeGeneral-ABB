using System;

namespace ABB.Application.PostingMutasiKlaims.Queries
{
    public class PostingMutasiKlaimDto
    {
        public int Id { get; set; }
        
        public string nm_cb { get; set; }

        public string nm_cob { get; set; }

        public string nm_scob { get; set; }
        
        public string kd_cb { get; set; }

        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public string kd_thn { get; set; }

        public string no_kl { get; set; }

        public Int16 no_mts { get; set; }

        public string nomor_register { get; set; }

        public string tipe_mts { get; set; }

        public string nm_tipe_mts { get; set; }

        public string nm_ttj { get; set; }

        public DateTime tgl_nt { get; set; }

        public string? no_pol_lama { get; set; }
    }
}