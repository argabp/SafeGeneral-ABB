using System.Collections.Generic;

namespace ABB.Application.DashboardKlaims.Queries
{
    public class DashboardKlaimDto
    {
        public DashboardKlaimDto()
        {
            Data = new List<DashboardKlaimDataDto>();
            Graphic = new List<DashboardKlaimGraphicDto>();
        }
        
        public List<DashboardKlaimDataDto> Data { get; set; }
        
        public List<DashboardKlaimGraphicDto> Graphic { get; set; }
    }
}