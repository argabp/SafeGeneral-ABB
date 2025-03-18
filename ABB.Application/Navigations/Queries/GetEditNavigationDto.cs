using System.Collections.Generic;

namespace ABB.Application.Navigations.Queries
{
    public class GetEditNavigationDto
    {
        public int NavigationId { get; set; }
        public string Text { get; set; }
        public string Icon { get; set; }
        public bool IsActive { get; set; }
        public int RouteId { get; set; }
        public List<int> SubNavigationId { get; set; }
    }
}