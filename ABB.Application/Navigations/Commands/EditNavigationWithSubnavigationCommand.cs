using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;

namespace ABB.Application.Navigations.Commands
{
    public class EditNavigationWithSubnavigationCommand : IRequest<int>
    {
        public int NavigationId { get; set; }
        public string Text { get; set; }
        public int RouteId { get; set; }
        public bool IsActive { get; set; }
        public string Icon { get; set; }
        public List<int> SubNavigationId { get; set; }
    }

    public class EditNavigationWithSubnavigationCommandHandler : IRequestHandler<EditNavigationWithSubnavigationCommand, int>
    {
        private readonly IDbContext _context;

        public EditNavigationWithSubnavigationCommandHandler(IDbContext context)
        {
            _context = context;
        }
        public async Task<int> Handle(EditNavigationWithSubnavigationCommand request, CancellationToken cancellationToken)
        {
            var editedNav = _context.Navigation.Where(x => x.NavigationId == request.NavigationId).SingleOrDefault();
            editedNav.Text = request.Text;
            editedNav.IsActive = request.IsActive;
            editedNav.Icon = request.Icon;
            editedNav.RouteId = request.RouteId;

            var childrenNav = _context.Navigation.Where(x => x.ParentId == request.NavigationId);
            foreach (var subnav in childrenNav) {
                subnav.ParentId = 0;
            }
            foreach (var subId in request.SubNavigationId) {
                var subnav = _context.Navigation.SingleOrDefault(x => x.NavigationId == subId);
                subnav.ParentId = request.NavigationId;
            }
            await _context.SaveChangesAsync(cancellationToken);

            return request.NavigationId;
        }
    }
}