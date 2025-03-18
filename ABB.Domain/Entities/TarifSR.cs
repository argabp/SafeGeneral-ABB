using System;

namespace ABB.Domain.Entities
{
    public class TarifSR
    {
        public string kd_cb { get; set; }

        public string kd_rk { get; set; }

        public int masa_ptg { get; set; }

        public int usia_awal { get; set; }

        public int usia_akhr { get; set; }

        public decimal pst_rate { get; set; }

        public Int16 stn_rate { get; set; }

        public Int16? jns_program { get; set; }
    }
}