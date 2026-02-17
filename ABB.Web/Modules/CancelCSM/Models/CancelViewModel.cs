using System.Collections.Generic;
using ABB.Application.CancelCSM.Queries;

namespace ABB.Web.Modules.CancelCSM.Models
{
    public class CancelViewModel
    {
        public List<CancelCSMDto> Datas { get; set; }
        
        public string KodeMetode { get; set; }
    }
}