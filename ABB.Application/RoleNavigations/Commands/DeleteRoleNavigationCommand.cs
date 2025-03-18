using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace ABB.Application.RoleNavigations.Commands
{
    public class DeleteRoleNavigationCommand : IRequest
    {
        public string Id { get; set; }
    }

    public class DeleteRoleNavigationCommandHandler : IRequestHandler<DeleteRoleNavigationCommand>
    {
        private readonly IRoleNavigationService _roleNavigationService;

        public DeleteRoleNavigationCommandHandler(IRoleNavigationService roleNavigationService)
        {
            _roleNavigationService = roleNavigationService;
        }
        public async Task<Unit> Handle(DeleteRoleNavigationCommand request, CancellationToken cancellationToken)
        {
            _roleNavigationService.DeleteRoleNaavigations(request.Id);
            _roleNavigationService.DeleteRoleRoutes(request.Id);

            await _roleNavigationService.Commit(cancellationToken);

            return Unit.Value;
        }
    }
}