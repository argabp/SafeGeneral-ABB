using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using ABB.Domain.Entities;

namespace ABB.Application.RoleNavigations
{
    public class RoleNavigationService : IRoleNavigationService
    {
        private readonly IDbContext _context;

        private List<RoleNavigation> RoleNavigations { get; set; }

        private List<RoleRoute> RoleRoutes { get; set; }

        public RoleNavigationService(IDbContext context)
        {
            RoleNavigations = new List<RoleNavigation>();
            RoleRoutes = new List<RoleRoute>();
            _context = context;
        }

        public void AddRoleNavigation(string roleId, int navId)
        {
            if (!RoleNavigations.Any(a => a.RoleId == roleId && a.NavigationId == navId) &&
                !_context.RoleNavigation.Any(a => a.RoleId == roleId && a.NavigationId == navId))
            {
                RoleNavigations.Add(new RoleNavigation()
                {
                    RoleId = roleId,
                    NavigationId = navId
                });
            }
        }

        public void AddRoleRouteInSimilarController(string roleId, List<string> controllerNames)
        {
            foreach (var controllerName in controllerNames)
            {
                var routes = _context.Route.Where(w => w.Controller == controllerName).ToList();
                if (routes.Any())
                {
                    foreach (var route in routes)
                    {
                        if (!RoleRoutes.Any(a => a.RoleId == roleId && a.RouteId == route.RouteId) &&
                            !_context.RoleRoute.Any(a => a.RoleId == roleId && a.RouteId == route.RouteId))
                        {
                            RoleRoutes.Add(new RoleRoute()
                            {
                                RoleId = roleId,
                                RouteId = route.RouteId
                            });
                        }
                    }
                }

                AddOtherRoleRouteWhenWorkspaceController(roleId, controllerName);
            }
        }

        private void AddOtherRoleRouteWhenWorkspaceController(string roleId, string controllerName)
        {
            if (controllerName == "Workspace")
            {
                var controllerNames = new List<string>();
                controllerNames.AddRange(new string[]
                {
                    "Role", "WorkspaceTeam", "EForm", "RoleEForm", "AssigneeRules", "NotificationSetting",
                    "EscalationRules", "DataSource"
                });

                AddRoleRouteInSimilarController(roleId, controllerNames);
            }
        }

        public void SaveRoleNavigation()
        {
            _context.RoleNavigation.AddRange(RoleNavigations);
        }

        public void SaveRoleRoute()
        {
            _context.RoleRoute.AddRange(RoleRoutes);
        }

        public async Task<int> Commit(CancellationToken cancellationToken)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }

        public void DeleteRoleNaavigations(string roleId)
        {
            var roleNavigations = _context.RoleNavigation.Where(w => w.RoleId == roleId).ToList();

            _context.RoleNavigation.RemoveRange(roleNavigations);
        }

        public void DeleteRoleRoutes(string roleId)
        {
            var roleRoute = _context.RoleRoute.Where(w => w.RoleId == roleId).ToList();

            _context.RoleRoute.RemoveRange(roleRoute);
        }

        public string GetControllerName(string roleId, int navId)
        {
            var routeId = _context.Navigation.Find(navId)?.RouteId;

            return _context.Route.FirstOrDefault(w => w.RouteId == routeId)?.Controller;
        }
    }
}