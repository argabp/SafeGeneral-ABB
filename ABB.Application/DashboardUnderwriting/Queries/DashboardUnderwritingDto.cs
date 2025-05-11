using System.Collections.Generic;

namespace ABB.Application.DashboardUnderwriting.Queries
{
    public class DashboardUnderwritingDto
    {
        public DashboardUnderwritingDto()
        {
            Data = new List<DashboardUnderwritingDataDto>();
            Graphic = new List<DashboardUnderwritingGraphicDto>();
        }
        
        public List<DashboardUnderwritingDataDto> Data { get; set; }
        
        public List<DashboardUnderwritingGraphicDto> Graphic { get; set; }
    }
}