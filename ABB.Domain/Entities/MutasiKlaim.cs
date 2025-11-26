using System;

namespace ABB.Domain.Entities
{
    public class MutasiKlaim
    {
        public string kd_cb { get; set; }

        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public string kd_thn { get; set; }

        public string no_kl { get; set; }

        public Int16 no_mts { get; set; }

        public DateTime tgl_mts { get; set; }

        public string tipe_mts { get; set; }

        public string kd_mtu { get; set; }

        public decimal nilai_ttl_kl { get; set; }

        public string flag_konv { get; set; }

        public DateTime tgl_input { get; set; }

        public string kd_usr_input { get; set; }

        public DateTime? tgl_updt { get; set; }

        public string? kd_usr_updt { get; set; }

        public DateTime? tgl_closing { get; set; }

        public string? kd_usr_closing { get; set; }

        public string flag_closing { get; set; }

        public DateTime? tgl_reas { get; set; }

        public string? kd_usr_reas { get; set; }

        public string flag_reas { get; set; }

        public byte? no_grp_mts { get; set; }

        public string? validitas { get; set; }
    }
}