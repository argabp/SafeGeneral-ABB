using System;

namespace ABB.Application.EntriNotaKlaims.Queries
{
    public class EntriNotaKlaimDto
    {
        public int Id { get; set; }
        
        public string kd_cb { get; set; }

        public string jns_tr { get; set; }

        public string jns_nt_msk { get; set; }

        public string kd_thn { get; set; }

        public string kd_bln { get; set; }

        public string no_nt_msk { get; set; }

        public string jns_nt_kel { get; set; }

        public string no_nt_kel { get; set; }

        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public string nm_cb { get; set; }

        public string nm_cob { get; set; }

        public string nm_scob { get; set; }

        public string nomor_register { get; set; }

        public string tipe_mts { get; set; }

        public string nm_tipe_mts { get; set; }

        public string nm_ttj { get; set; }

        public DateTime tgl_nt { get; set; }

        public Int16 no_mts { get; set; }

        public string no_kl { get; set; }

        public string flag_posting { get; set; }

        public string? no_pol_lama { get; set; }

        public decimal? nilai_nt { get; set; }
    }
}