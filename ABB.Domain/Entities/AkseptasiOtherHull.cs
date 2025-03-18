using System;

namespace ABB.Domain.Entities
{
    public class AkseptasiOtherHull
    {
        public string kd_cb { get; set; }

        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public string kd_thn { get; set; }

        public string no_aks { get; set; }

        public Int16 no_updt { get; set; }

        public Int16 no_rsk { get; set; }

        public string kd_endt { get; set; }

        public string kd_kapal { get; set; }

        public string? nm_kapal { get; set; }

        public string? merk_kapal { get; set; }

        public string no_rangka { get; set; }

        public string no_msn { get; set; }

        public decimal? thn_buat { get; set; }

        public int? grt { get; set; }

        public string? no_pol_ttg { get; set; }
    }
}