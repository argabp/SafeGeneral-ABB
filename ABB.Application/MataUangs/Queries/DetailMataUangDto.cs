using System;

namespace ABB.Application.MataUangs.Queries
{
    public class DetailMataUangDto
    {
        public string Id { get; set; }
        
        public string kd_mtu { get; set; }

        public DateTime tgl_mul { get; set; }
        
        public DateTime tgl_akh { get; set; }

        public decimal nilai_kurs { get; set; }
    }
}