using System;

namespace ABB.Domain.Entities
{
    public class PesertaLampiran
    {
        public string kd_cb { get; set; }

        public string kd_product { get; set; }

        public string kd_thn { get; set; }

        public string kd_rk { get; set; }

        public string no_sppa { get; set; }

        public Int16 no_updt { get; set; }

        public Int16 no_dokumen { get; set; }

        public string dokumen { get; set; }

        public bool status { get; set; }

        public string flag_finish { get; set; }
    }
}