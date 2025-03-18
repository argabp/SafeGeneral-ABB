using System;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;

namespace ABB.Application.Roles.Queries
{
    public class GetMaxRoleCodeQuery : IRequest<int>
    {
    }

    public class GetMaxRoleCodeHandler : IRequestHandler<GetMaxRoleCodeQuery, int>
    {
        private readonly IDbConnection _db;

        public GetMaxRoleCodeHandler(IDbConnection db)
        {
            _db = db;
        }
        public async Task<int> Handle(GetMaxRoleCodeQuery request, CancellationToken cancellationToken)
        {
            return Convert.ToInt32(await _db.ExecuteScalar("SELECT MAX(RoleCode) FROM MS_Role WHERE ISNULL(IsDeleted,0) = 0"));
        }

    }
}