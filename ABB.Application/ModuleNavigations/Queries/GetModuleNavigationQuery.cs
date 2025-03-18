using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;

namespace ABB.Application.ModuleNavigations.Queries
{
    public class GetModuleNavigationQuery : IRequest<ModuleNavigationDto>
    {
        public int ModuleId { get; set; }
    }
    
    public class GetModuleNavigationQueryHandler : IRequestHandler<GetModuleNavigationQuery, ModuleNavigationDto>
    {
        private readonly IDbContext _dbContext;
    
        public GetModuleNavigationQueryHandler(IDbContext dbContext)
        {
            _dbContext = dbContext;
        }
    
        public async Task<ModuleNavigationDto> Handle(GetModuleNavigationQuery request, CancellationToken cancellationToken)
        {
            await Task.Delay(0, cancellationToken);

            var moduleNavigations = _dbContext.ModuleNavigation
                .Where(moduleNavigation => moduleNavigation.ModuleId == request.ModuleId).ToList();

            return new ModuleNavigationDto()
            {
                ModuleId = request.ModuleId,
                Navigations = moduleNavigations.Select(moduleNavigation => moduleNavigation.NavigationId).ToList()
            };
        }
    }
}