using System.Collections.Generic;
using ABB.Application.ProsesCSM.Queries;

namespace ABB.Web.Modules.ProsesCSM.Models
{
    public class ProsesViewModel
    {
        public List<ProsesCSMDto> Datas { get; set; }
        
        public string KodeMetode { get; set; }
    }
}