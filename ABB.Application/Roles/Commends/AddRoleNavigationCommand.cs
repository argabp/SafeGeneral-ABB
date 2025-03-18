using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using ABB.Domain.Entities;
using MediatR;

namespace ABB.Application.Roles.Commends
{
    public class AddRoleNavigationCommand : IRequest
    {
        public int NavigationId { get; set; } = 0;
        public string RoleId { get; set; }
        public string Text { get; set; }
    }

    public class AddRoleNavigationCommandHandler : IRequestHandler<AddRoleNavigationCommand>
    {
        private readonly IDbContext _context;

        public AddRoleNavigationCommandHandler(IDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(AddRoleNavigationCommand request, CancellationToken cancellationToken)
        {
            var navIdFromText = _context.Navigation.FirstOrDefault(a => a.Text == request.Text)?.NavigationId ?? 0;
            var navId = request.NavigationId == 0 ? navIdFromText : request.NavigationId;
            await _context.RoleNavigation.AddAsync(new RoleNavigation
                { NavigationId = navId, RoleId = request.RoleId });
            await _context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}