using System;

namespace ABB.Application.KlaimAlokasiReasuransis.Queries
{
    public class KlaimAlokasiReasuransiDto
    {
        public string Id { get; set; }
        public string kd_cb { get; set; }
        public string kd_cob { get; set; }
        public string kd_scob { get; set; }
        public string kd_thn { get; set; }
        public string no_kl { get; set; }
        public short no_mts { get; set; }
        public string kd_jns_sor { get; set; }
        public string kd_grp_sor { get; set; }
        public string kd_rk_sor { get; set; }
        public string nm_jns_sor { get; set; }
        public string nm_rk_sor { get; set; }
        public decimal pst_share { get; set; }
        public decimal nilai_kl { get; set; }
        public string flag_cash_call { get; set; }
        public string flag_nota { get; set; }
        public string? flag_nt { get; set; }
    }
}