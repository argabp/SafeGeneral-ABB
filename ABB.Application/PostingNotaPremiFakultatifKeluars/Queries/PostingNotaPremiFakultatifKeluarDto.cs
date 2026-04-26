using System;

namespace ABB.Application.PostingNotaPremiFakultatifKeluars.Queries
{
    public class PostingNotaPremiFakultatifKeluarDto
    {
        public string Id { get; set; }
        
        public string nomor_nota { get; set; }

        public string kd_cb { get; set; }

        public string nm_cb { get; set; }

        public string kd_cob { get; set; }
        
        public string nm_cob { get; set; }
        
        public string nm_scob { get; set; }

        public string kd_scob { get; set; }

        public string nm_ttj { get; set; }

        public string nm_ttg { get; set; }

        public string no_pol { get; set; }
        
        public short no_rsk { get; set; }

        public DateTime tgl_nt { get; set; }

        public string nilai_nt { get; set; }

        public string jns_tr { get; set; }

        public string jns_nt_msk { get; set; }

        public string kd_thn { get; set; }

        public string kd_bln { get; set; }

        public string no_nt_msk { get; set; }

        public string jns_nt_kel { get; set; }

        public string no_nt_kel { get; set; }

        public DateTime tgl_posing { get; set; }

        public string kd_usr_posting { get; set; }
    }
}