using System;

namespace ABB.Application.LossRecord.Queries
{
    public class LossRecordDto
    {
        public string? nm_cb { get; set; }

        public string? nm_ttg { get; set; }

        public DateTime? tgl_mul { get; set; }

        public DateTime? tgl_akh { get; set; }

        public string? no_berkas { get; set; }

        public DateTime? tgl_kej { get; set; }

        public string? kd_mtu_symbol { get; set; }

        public decimal? nilai_kl { get; set; }

        public string? nm_cob { get; set; }

        public string? status { get; set; }
    }
}