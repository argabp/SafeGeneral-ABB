using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Exceptions;
using ABB.Application.Common.Interfaces;
using ABB.Domain.IdentityModels;
using MediatR;

namespace ABB.Application.Roles.Queries
{
    public class GetRoleQuery : IRequest<RolesDto>
    {
        public string Id { get; set; }
    }

    public class GetRoleQueryHandler : IRequestHandler<GetRoleQuery, RolesDto>
    {
        private readonly IDbContext _context;

        public GetRoleQueryHandler(IDbContext context)
        {
            _context = context;
        }

        public async Task<RolesDto> Handle(GetRoleQuery request, CancellationToken cancellationToken)
        {
            var role = await _context.Role.FindAsync(request.Id);

            if (role == null)
                throw new NotFoundException(nameof(AppRole), request.Id);

            var result = new RolesDto()
            {
                RoleId = role.Id,
                RoleCode = role.RoleCode,
                RoleName = role.Name,
                Description = role.Description
            };

            return result;
        }
    }
}