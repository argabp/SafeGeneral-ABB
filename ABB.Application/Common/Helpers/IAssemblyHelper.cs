using System.Collections.Generic;
using ABB.Domain.Entities;

namespace ABB.Application.Common.Helpers
{
    public interface IAssemblyHelper
    {
        List<Route> GetAssemblyRoutes();
    }
}