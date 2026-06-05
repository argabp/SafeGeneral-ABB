using System;

namespace ABB.Application.CancelPostingNotaPremiFakultatifKeluars.Queries
{
    public class CancelPostingNotaPremiFakultatifKeluarDto
    {
        public string nomor_nota { get; set; }
        
        public string nm_ttj { get; set; }

        public string nm_ttg { get; set; }

        public string no_pol { get; set; }

        public string no_pol_ttg { get; set; }

        public string nilai_nt { get; set; }
        
        public string jns_sb_nt { get; set; }
        
        public string kd_cb { get; set; }

        public string jns_tr { get; set; }

        public string jns_nt_msk { get; set; }

        public string kd_thn { get; set; }

        public string kd_bln { get; set; }

        public string no_nt_msk { get; set; }
        
        public string jns_nt_kel { get; set; }
        
        public string no_nt_kel { get; set; }

        public DateTime tgl_nt { get; set; }
    }
}