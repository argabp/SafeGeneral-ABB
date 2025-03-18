using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using ABB.Domain.Entities;
using MediatR;

namespace ABB.Application.RoleLevels.Commends
{
    public class AddRoleLevelCommand : IRequest
    {
        public string RoleId { get; set; }
        public string ParentId { get; set; }
        public int RoleCode { get; set; }
        public int ParentCode { get; set; }
    }

    public class AddRoleLevelCommandHandler : IRequestHandler<AddRoleLevelCommand>
    {
        private readonly IDbContext _context;

        public AddRoleLevelCommandHandler(IDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(AddRoleLevelCommand request, CancellationToken cancellationToken)
        {
            var roleId = _context.Role.FirstOrDefault(a => a.RoleCode == request.RoleCode)?.Id;
            var parentRoleId = _context.Role.FirstOrDefault(a => a.RoleCode == request.ParentCode)?.Id;
            var exist = _context.RoleLevel.Any(a => a.RoleId == roleId);
            if (!exist)
            {
                await _context.RoleLevel.AddAsync(new RoleLevel
                {
                    RoleId = roleId,
                    ParentId = parentRoleId
                });
            }

            await _context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}