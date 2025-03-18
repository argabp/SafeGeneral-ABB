using System.Collections.Generic;

namespace ABB.Application.ModuleNavigations.Queries
{
    public class ModuleNavigationDto
    {
        public int ModuleId { get; set; }

        public List<int> Navigations { get; set; }
    }
}