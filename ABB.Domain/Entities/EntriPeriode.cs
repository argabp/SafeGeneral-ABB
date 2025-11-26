using System;

namespace ABB.Domain.Entities
{
    public class EntriPeriode
    {
        public decimal ThnPrd { get; set; } 

        public short BlnPrd { get; set; } 

        public DateTime? TglMul { get; set; }
        public DateTime? TglAkh { get; set; }
        
        public string FlagClosing { get; set; } 
    }
}