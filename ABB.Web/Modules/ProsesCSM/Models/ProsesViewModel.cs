using System.Collections.Generic;

namespace ABB.Web.Modules.ProsesCSM.Models
{
    public class ProsesViewModel
    {
        public List<long> Id { get; set; }

        public string TipeTransaksi { get; set; }
        
        public string KodeMetode { get; set; }
    }
}