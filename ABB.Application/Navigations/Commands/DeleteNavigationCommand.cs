using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;

namespace ABB.Application.Navigations.Commands
{
    public class DeleteNavigationCommand : IRequest<int>
    {
        public int NavigationId { get; set; }
    }

    public class DeleteNavigationCommandHandler : IRequestHandler<DeleteNavigationCommand, int>
    {
        private readonly IDbContext _context;

        public DeleteNavigationCommandHandler(IDbContext context)
        {
            _context = context;
        }
        public async Task<int> Handle(DeleteNavigationCommand request, CancellationToken cancellationToken)
        {

            var deleteNav = _context.Navigation.Where(x=>x.NavigationId==request.NavigationId).SingleOrDefault();
            var subnav = _context.Navigation.Where(x => x.ParentId == deleteNav.NavigationId);
            foreach (var subnavItem in subnav) {
                subnavItem.ParentId = 0;
            }
            _context.Navigation.Remove(deleteNav);

            await _context.SaveChangesAsync(cancellationToken);

            return deleteNav.NavigationId;
        }
    }
}