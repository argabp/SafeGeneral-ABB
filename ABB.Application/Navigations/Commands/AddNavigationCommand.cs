using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using ABB.Domain.Entities;
using MediatR;

namespace ABB.Application.Navigations.Commands
{
    public class AddNavigationCommand : IRequest<int>
    {
        public int NavigationId { get; set; }
        public string Text { get; set; }
        public int ParentId { get; set; }
        public int RouteId { get; set; } = 0;
        public bool IsActive { get; set; } = true;
        public int Sort { get; set; } = 0;
        public string Icon { get; set; } = "fa-circle";
        public string Controller { get; set; } = "";
        public string Action { get; set; } = "";
    }

    public class AddNavigationCommandHandler : IRequestHandler<AddNavigationCommand, int>
    {
        private readonly IDbContext _context;

        public AddNavigationCommandHandler(IDbContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(AddNavigationCommand request, CancellationToken cancellationToken)
        {
            var item = new Navigation
            {
                Text = request.Text,
                ParentId = request.ParentId,
                IsActive = request.IsActive,
                Icon = request.Icon,
                Sort = request.Sort
            };
            var routeId = _context.Route
                .FirstOrDefault(a => a.Action == request.Action && a.Controller == request.Controller)?.RouteId ?? -1;
            item.RouteId = request.RouteId == 0 ? routeId : request.RouteId;
            await _context.Navigation.AddAsync(item);
            await _context.SaveChangesAsync(cancellationToken);
            return item.NavigationId;
        }
    }
}