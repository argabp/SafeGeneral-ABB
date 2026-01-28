using System.Collections.Generic;

namespace ABB.Application.Common.Grids.Models
{
    public class FilterGroup
    {
        public string logic { get; set; } // and/or
        public List<FilterNode> filters { get; set; }
    }
}