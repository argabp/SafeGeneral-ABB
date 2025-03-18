using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using ABB.Domain.Entities;
using AutoMapper;
using MediatR;

namespace ABB.Application.RoleModules.Commands
{
    public class SaveRoleModuleCommand : IRequest
    {
        public string RoleId { get; set; }

        public List<int> Modules { get; set; }
    }

    public class SaveRoleModuleCommandHandler : IRequestHandler<SaveRoleModuleCommand>
    {
        private readonly IDbContext _context;
        private readonly IMapper _mapper;

        public SaveRoleModuleCommandHandler(IDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Unit> Handle(SaveRoleModuleCommand request, CancellationToken cancellationToken)
        {
            var roleModule = _context.RoleModule.Where(roleModule => roleModule.RoleId == request.RoleId);
            
            _context.RoleModule.RemoveRange(roleModule);

            foreach (var moduleId in request.Modules)
                _context.RoleModule.Add(new RoleModule()
                {
                    RoleId = request.RoleId,
                    ModuleId = moduleId
                });

            await _context.SaveChangesAsync(cancellationToken);
            
            return Unit.Value;
        }
    }
}