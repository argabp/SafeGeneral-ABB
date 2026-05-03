using System;

namespace ABB.Domain.Entities
{
    public class ProsesPremiXOLKeluar
    {
        public string kd_cb { get; set; }

        public string kd_thn { get; set; }

        public string kd_bln { get; set; }

        public string kd_jns_sor { get; set; }

        public string kd_tty_npps { get; set; }

        public string kd_mtu { get; set; }

        public string no_tr { get; set; }

        public DateTime tgl_closing { get; set; }

        public string? ket_tr { get; set; }

        public string no_ref { get; set; }

        public decimal nilai_prm { get; set; }

        public string kd_usr_input { get; set; }

        public DateTime tgl_input { get; set; }

        public string flag_closing { get; set; }
    }
}