using System;

namespace ABB.Domain.Entities
{
    public class NotaKlaimTreaty
    {
        public string kd_cb { get; set; }

        public string jns_tr { get; set; }

        public string jns_nt_msk { get; set; }

        public string kd_thn { get; set; }

        public string kd_bln { get; set; }

        public string no_nt_msk { get; set; }
        
        public string jns_nt_kel { get; set; }
        
        public string no_nt_kel { get; set; }

        public string kd_cob { get; set; }

        public string kd_jns_tty { get; set; }

        public string kd_rk_tty { get; set; }

        public string kd_mtu { get; set; }

        public string kd_grp_pas { get; set; }

        public string kd_rk_pas { get; set; }

        public string? ket_nt { get; set; }

        public decimal nilai_nt { get; set; }

        public DateTime tgl_nt { get; set; }

        public string flag_cancel { get; set; }

        public string flag_posting { get; set; }

        public string? kd_cb_pol { get; set; }
    }
}