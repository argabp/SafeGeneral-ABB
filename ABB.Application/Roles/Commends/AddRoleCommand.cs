using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Dtos;
using ABB.Application.Common.Helpers;
using ABB.Application.Common.Interfaces;
using ABB.Application.Common.Services;
using ABB.Domain.Enums;
using ABB.Domain.IdentityModels;
using MediatR;

namespace ABB.Application.Roles.Commends
{
    public class AddRoleCommand : IRequest<string>
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int RoleCode { get; set; }
        public string Description { get; set; }
        public string UserId { get; set; }
    }

    public class AddRoleCommandHandler : IRequestHandler<AddRoleCommand, string>
    {
        private readonly IAuditTrailService _auditService;
        private readonly IDbContext _context;
        private readonly IRoleManagerHelper _roleManager;

        public AddRoleCommandHandler(IRoleManagerHelper roleManager, IAuditTrailService auditServic, IDbContext context)
        {
            _roleManager = roleManager;
            _auditService = auditServic;
            _context = context;
        }

        public async Task<string> Handle(AddRoleCommand request, CancellationToken cancellationToken)
        {
            var role = _context.Role.FirstOrDefault(w => (w.Name == request.Name || w.RoleCode == request.RoleCode));

            if (role == null)
            {
                var item = new AppRole()
                {
                    Name = request.Name,
                    Description = request.Description,
                    RoleCode = request.RoleCode,
                    IsDeleted = false
                };

                var success = await _roleManager.CreateAsync(item);

                var auditTrail = new AuditTrailDto()
                {
                    AuditEvent = AuditEvent.RoleCreation,
                    Status = success ? AuditStatus.Success : AuditStatus.Failed,
                    Platform = AuditPlatform.Web,
                    UserId = request.UserId
                };

                await _auditService.Create(auditTrail);

                return item.Id;
            }
            else
            {
                role.Name = request.Name;
                role.Description = request.Description;
                role.IsDeleted = false;
                role.RoleCode = request.RoleCode;

                await _context.SaveChangesAsync(cancellationToken);

                return role.Id;
            }
        }
    }
}