using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;

namespace ABB.Application.RoleModules.Queries
{
    public class GetRoleModuleQuery : IRequest<RoleModuleDto>
    {
        public string RoleId { get; set; }
    }
    
    public class GetRoleModuleQueryHandler : IRequestHandler<GetRoleModuleQuery, RoleModuleDto>
    {
        private readonly IDbContext _dbContext;
    
        public GetRoleModuleQueryHandler(IDbContext dbContext)
        {
            _dbContext = dbContext;
        }
    
        public async Task<RoleModuleDto> Handle(GetRoleModuleQuery request, CancellationToken cancellationToken)
        {
            await Task.Delay(0, cancellationToken);
            
            var roleModules = _dbContext.RoleModule.Where(roleModule => roleModule.RoleId == request.RoleId).ToList();

            return new RoleModuleDto()
            {
                RoleId = request.RoleId,
                Modules = roleModules.Select(roleModule => roleModule.ModuleId).ToList()
            };
        }
    }
}