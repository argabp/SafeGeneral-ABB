using System.Threading.Tasks;
using ABB.Application.Common.Grids.Models;

namespace ABB.Application.Common.Grids.Interfaces
{
    public interface IGridQueryEngine
    {
        Task<GridResponse<T>> QueryAsync<T>(
            GridRequest request,
            GridConfig config,
            object parameters,
            string databaseName);

        Task<GridResponse<T>> QueryAsyncCSM<T>(GridRequest request, GridConfig config, 
        object parameters);
    }
}