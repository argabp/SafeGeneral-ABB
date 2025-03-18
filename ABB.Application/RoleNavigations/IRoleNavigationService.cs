using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ABB.Application.RoleNavigations
{
    public interface IRoleNavigationService
    {
        void AddRoleNavigation(string roleId, int navId);

        void AddRoleRouteInSimilarController(string roleId, List<string> controllerNames);

        void DeleteRoleNaavigations(string roleId);

        void DeleteRoleRoutes(string roleId);

        void SaveRoleNavigation();

        void SaveRoleRoute();

        string GetControllerName(string roleId, int navId);

        Task<int> Commit(CancellationToken cancellationToken);
    }
}