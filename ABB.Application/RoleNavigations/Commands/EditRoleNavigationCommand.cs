using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace ABB.Application.RoleNavigations.Commands
{
    public class EditRoleNavigationCommand : IRequest
    {
        public EditRoleNavigationCommand()
        {
            Navigations = new List<int>();
        }

        public string RoleId { get; set; }
        public List<int> Navigations { get; set; }
    }

    public class EditRoleNavigationCommandHandler : IRequestHandler<EditRoleNavigationCommand>
    {
        private readonly IRoleNavigationService _roleNavigationService;

        public EditRoleNavigationCommandHandler(IRoleNavigationService roleNavigationService)
        {
            _roleNavigationService = roleNavigationService;
        }

        public async Task<Unit> Handle(EditRoleNavigationCommand request, CancellationToken cancellationToken)
        {
            _roleNavigationService.DeleteRoleNaavigations(request.RoleId);
            _roleNavigationService.DeleteRoleRoutes(request.RoleId);

            await _roleNavigationService.Commit(cancellationToken);

            var controllerNames = new List<string>();

            foreach (var navId in request.Navigations)
            {
                _roleNavigationService.AddRoleNavigation(request.RoleId, navId);
                controllerNames.Add(_roleNavigationService.GetControllerName(request.RoleId, navId));
            }

            _roleNavigationService.SaveRoleNavigation();

            _roleNavigationService.AddRoleRouteInSimilarController(request.RoleId, controllerNames.Distinct().ToList());

            _roleNavigationService.SaveRoleRoute();

            await _roleNavigationService.Commit(cancellationToken);

            return Unit.Value;
        }
    }
}