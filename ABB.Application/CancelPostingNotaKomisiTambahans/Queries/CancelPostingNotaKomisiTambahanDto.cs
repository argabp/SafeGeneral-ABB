using System;

namespace ABB.Application.CancelPostingNotaKomisiTambahans.Queries
{
    public class CancelPostingNotaKomisiTambahanDto
    {
        public string jns_sb_nt { get; set; }
        
        public string kd_cb { get; set; }

        public string jns_tr { get; set; }

        public string jns_nt_msk { get; set; }

        public string kd_thn { get; set; }

        public string kd_bln { get; set; }

        public string no_nt_msk { get; set; }

        public string jns_nt_kel { get; set; }

        public string no_nt_kel { get; set; }

        public string nm_ttj { get; set; }

        public string flag_posting { get; set; }
        
        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public string no_pol { get; set; }

        public Int16 no_updt { get; set; }
        
        public DateTime? tgl_posting { get; set; }

        public DateTime? tgl_closing { get; set; }
        
        public string? kd_usr_posting { get; set; }

        public string? no_pol_ttg { get; set; }
    }
}