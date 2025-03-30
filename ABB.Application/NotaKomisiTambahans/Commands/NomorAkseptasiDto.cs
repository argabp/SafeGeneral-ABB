using System;

namespace ABB.Application.NotaKomisiTambahans.Commands
{
    public class NomorAkseptasiDto
    {
        public int Id { get; set; }
        public string kd_cb { get; set; }
        public string kd_cob { get; set; }
        public string kd_scob { get; set; }
        public string kd_thn { get; set; }
        public string no_pol { get; set; }
        public Int16 no_updt { get; set; }
        public string nm_ttg { get; set; }
        public string kd_grp_sb_bis { get; set; }
        public string kd_rk_sb_bis { get; set; }
        public string kd_mtu { get; set; }
        public string no_pol_ttg { get; set; }

        public string nm_cb { get; set; }
        public string nm_cob { get; set; }
        public string nm_scob { get; set; }
        public string no_akseptasi { get; set; }
    }
}