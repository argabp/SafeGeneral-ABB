using System;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Dtos;
using ABB.Application.Common.Exceptions;
using ABB.Application.Common.Interfaces;
using ABB.Application.Common.Services;
using ABB.Domain.Enums;
using ABB.Domain.IdentityModels;
using MediatR;

namespace ABB.Application.Roles.Commends
{
    public class EditRoleCommand : IRequest
    {
        public EditRoleCommand()
        {
            this.Id = Guid.NewGuid().ToString("N");
        }

        public string Id { get; set; }
        public int WorkspaceId { get; set; }
        public string Name { get; set; }
        public int RoleCode { get; set; }
        public string Description { get; set; }

        public string UserId { get; set; }
    }

    public class EditRoleCommandHandler : IRequestHandler<EditRoleCommand>
    {
        private readonly IDbContext _context;
        private readonly IAuditTrailService _auditTrailService;

        public EditRoleCommandHandler(IDbContext context, IAuditTrailService auditTrailService)
        {
            _context = context;
            _auditTrailService = auditTrailService;
        }

        public async Task<Unit> Handle(EditRoleCommand request, CancellationToken cancellationToken)
        {
            var entity = _context.Role.Find(request.Id);
            if (entity == null)
            {
                throw new NotFoundException(nameof(AppRole), request.Id);
            }

            entity.Name = request.Name;
            entity.Description = request.Description;
            entity.RoleCode = request.RoleCode;
            entity.NormalizedName = entity.Name.ToUpper();

            var resultSave = await _context.SaveChangesAsync(cancellationToken);

            var auditTrail = new AuditTrailDto()
            {
                AuditEvent = AuditEvent.RoleModification,
                Status = resultSave == 1 ? AuditStatus.Success : AuditStatus.Failed,
                Platform = AuditPlatform.Web,
                UserId = request.UserId
            };

            await _auditTrailService.Create(auditTrail);

            return Unit.Value;
        }
    }
}