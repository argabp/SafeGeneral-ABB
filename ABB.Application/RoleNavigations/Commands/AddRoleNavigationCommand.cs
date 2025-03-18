using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using ABB.Domain.Entities;
using MediatR;

namespace ABB.Application.RoleNavigations.Commands
{
    public class AddRoleNavigationCommand : IRequest
    {
        public AddRoleNavigationCommand()
        {
            Navigations = new List<int>();
        }
        public string RoleId { get; set; }
        public List<int> Navigations { get; set; }
    }
    public class AddRoleNavigationCommandHandler : IRequestHandler<AddRoleNavigationCommand>
    {
        private readonly IRoleNavigationService _roleNavigationService;

        public AddRoleNavigationCommandHandler(IRoleNavigationService roleNavigationService)
        {
            _roleNavigationService = roleNavigationService;
        }
        public async Task<Unit> Handle(AddRoleNavigationCommand request, CancellationToken cancellationToken)
        {

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