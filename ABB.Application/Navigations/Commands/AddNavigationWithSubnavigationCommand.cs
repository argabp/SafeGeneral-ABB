using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using ABB.Domain.Entities;
using MediatR;

namespace ABB.Application.Navigations.Commands
{
    public class AddNavigationWithSubnavigationCommand : IRequest<int>
    {
        public string Text { get; set; }
        public int RouteId { get; set; }
        public bool IsActive { get; set; }
        public string Icon { get; set; }
        public List<int> SubNavigationId { get; set; }
    }

    public class
        AddNavigationWithSubnavigationCommandHandler : IRequestHandler<AddNavigationWithSubnavigationCommand, int>
    {
        private readonly IDbContext _context;

        public AddNavigationWithSubnavigationCommandHandler(IDbContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(AddNavigationWithSubnavigationCommand request,
            CancellationToken cancellationToken)
        {
            var navigationParent = new Navigation
            {
                Text = request.Text,
                ParentId = 0,
                IsActive = request.IsActive,
                Icon = request.Icon,
                RouteId = request.RouteId
            };
            _context.Navigation.Add(navigationParent);
            await _context.SaveChangesAsync(cancellationToken);
            var parentid = navigationParent.NavigationId;
            foreach (var subId in request.SubNavigationId)
            {
                var subnav = _context.Navigation.SingleOrDefault(x => x.NavigationId == subId);
                subnav.ParentId = parentid;
                await _context.SaveChangesAsync(cancellationToken);
            }

            return parentid;
        }
    }
}