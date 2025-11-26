using System;

namespace ABB.Application.MutasiKlaims.Queries
{
    public class MutasiKlaimDto
    {
        public int Id { get; set; }
        
        public string kd_cb { get; set; }

        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public string kd_thn { get; set; }

        public string no_kl { get; set; }

        public string nm_cb { get; set; }
        public string nm_cob { get; set; }
        public string nm_scob { get; set; }
        public string register_klaim { get; set; }

        public string id_detail_grid { get; set; }

        public string flag_pol_lama { get; set; }

        public string flag_tty_msk { get; set; }

        public string? no_pol_lama { get; set; }

        public string? kd_thn_pol { get; set; }

        public string? no_pol { get; set; }

        public Int16? no_updt { get; set; }

        public Int16? no_rsk { get; set; }

        public string? kd_jns_sor { get; set; }

        public string? kd_tty_msk { get; set; }

        public string? no_lks_lama { get; set; }

        public string flag_settled { get; set; }

        public string? st_jns_peny { get; set; }

        public DateTime tgl_lapor { get; set; }

        public DateTime tgl_kej { get; set; }

        public string? tempat_kej { get; set; }

        public string? sebab_kerugian { get; set; }

        public string? kond_ptg { get; set; }

        public string? kd_sebab { get; set; }

        public string? sifat_kerugian { get; set; }

        public DateTime tgl_lns_prm { get; set; }

        public string? no_bukti_lns { get; set; }

        public DateTime tgl_reg { get; set; }

        public string? ket_kl { get; set; }

        public DateTime tgl_input { get; set; }

        public string kd_usr_input { get; set; }

        public DateTime? tgl_updt { get; set; }

        public string? kd_usr_updt { get; set; }

        public string? nm_ttg { get; set; }
    }
}