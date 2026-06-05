using System;

namespace ABB.Application.CetakNotaPremiTreatyKeluars.Commands
{
    public class CetakNotaPremiTreatyKeluarModel
    {
        public string reasuradur { get; set; }
        
        public DateTime bl_prd { get; set; }

        public string thn_tty_pps { get; set; }

        public string nm_tty_pps { get; set; }

        public string kd_jns_sor { get; set; }

        public string no_nota { get; set; }

        public string symbol_mtu { get; set; }

        public decimal prm { get; set; }

        public decimal prm_bgn { get; set; }

        public decimal prm_sdr { get; set; }

        public decimal kms { get; set; }

        public decimal netto { get; set; }

        public decimal tot_prm_idr { get; set; }

        public decimal tot_prm_sdr_idr { get; set; }

        public decimal tot_kms_idr { get; set; }

        public decimal tot_netto_idr { get; set; }

        public decimal tot_prm_usd { get; set; }

        public decimal tot_prm_sdr_usd { get; set; }

        public decimal tot_kms_usd { get; set; }

        public decimal tot_netto_usd { get; set; }

        public string nm_cob { get; set; }

        public string? nm_bag { get; set; }

        public string? nm_kpl_bag { get; set; }
    }
}