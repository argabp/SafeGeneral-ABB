using System.Collections.Generic;

namespace ABB.Application.Common.Grids.Models
{
    public class FilterNode
    {
        public string field { get; set; }
        public string @operator { get; set; }
        public string value { get; set; }

        // nested group
        public string logic { get; set; }
        public List<FilterNode> filters { get; set; }
    }
}