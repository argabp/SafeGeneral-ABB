using System.Collections.Generic;

namespace ABB.Application.Common.Grids.Models
{
    public class GridConfig
    {
        public string FromSql { get; set; }
        public string BaseWhere { get; set; }
        
        public Dictionary<string, string> ColumnMap { get; set; } = new();
        public List<string> SearchableColumns { get; set; } = new();
    }
}