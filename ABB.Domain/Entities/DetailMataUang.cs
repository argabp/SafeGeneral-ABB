using System;

namespace ABB.Domain.Entities
{
    public class DetailMataUang
    {
        public string kd_mtu { get; set; }

        public DateTime tgl_mul { get; set; }
        
        public DateTime tgl_akh { get; set; }

        public decimal nilai_kurs { get; set; }
    }
}