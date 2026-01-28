using System.Collections.Generic;

namespace ABB.Application.Common.Grids.Models
{
    public class GridResponse<T>
    {
        public int Total { get; set; }
        public List<T> Data { get; set; }
    }
}