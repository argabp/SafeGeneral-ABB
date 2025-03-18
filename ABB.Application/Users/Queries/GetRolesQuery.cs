using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using ABB.Domain.IdentityModels;
using MediatR;

namespace ABB.Application.Users.Queries
{
    public class GetRolesQuery : IRequest<List<AppRole>>
    {
    }

    public class GetRolesQueryHandler : IRequestHandler<GetRolesQuery, List<AppRole>>
    {
        private readonly IDbConnection _db;
        public GetRolesQueryHandler(IDbConnection db)
        {
            _db = db;
        }
        public async Task<List<AppRole>> Handle(GetRolesQuery request, CancellationToken cancellationToken)
        {
            return (await _db.Query<AppRole>(@"SELECT RoleId Id, RoleName Name FROM MS_Role 
            WHERE ISNULL(IsDeleted,0)=0")).ToList();
        }
    }
}