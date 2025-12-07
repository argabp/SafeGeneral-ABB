using System;

namespace ABB.Application.DashboardKlaims.Queries
{
    public class DashboardKlaimGraphicDto
    {
        public DateTime posisi { get; set; }

        public string nm_cab { get; set; }

        public string nm_cob { get; set; }

        public decimal klaim_sdthnini { get; set; }

        public decimal target_rkap { get; set; }

        public decimal rasio { get; set; }

        public decimal klaim_sdthnlalu { get; set; }

        public decimal naik_turun { get; set; }

        public decimal klaim_blnini { get; set; }

        public decimal klaim_blnlalu { get; set; }

        public decimal naik_turun2 { get; set; }
    }
}