using System;

namespace ABB.Application.DashboardUnderwriting.Queries
{
    public class DashboardUnderwritingGraphicDto
    {
        public DateTime posisi { get; set; }

        public string nm_cab { get; set; }

        public string nm_cob { get; set; }

        public decimal produksi_sdthnini { get; set; }

        public decimal target_rkap { get; set; }

        public decimal rasio { get; set; }

        public decimal produksi_sdthnlalu { get; set; }

        public decimal naik_turun { get; set; }

        public decimal produksi_blnini { get; set; }

        public decimal produksi_blnlalu { get; set; }

        public decimal naik_turun2 { get; set; }
    }
}