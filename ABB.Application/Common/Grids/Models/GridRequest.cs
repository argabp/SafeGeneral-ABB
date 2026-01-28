namespace ABB.Application.Common.Grids.Models
{
    public class GridRequest
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;
        
        public string SortField { get; set; }
        public string SortDir { get; set; } // asc / desc
        
        public string SearchKeyword { get; set; }
        public string FiltersJson { get; set; }
    }
}