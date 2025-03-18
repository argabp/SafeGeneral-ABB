using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using ABB.Domain.Entities;
using MediatR;

namespace ABB.Application.Users.Queries
{
    public class GetCabangQuery : IRequest<Cabang>
    {
        public string kd_cb { get; set; }
    }

    public class GetCabangQueryHandler : IRequestHandler<GetCabangQuery, Cabang>
    {
        private readonly IDbContext _dbContext;
        public GetCabangQueryHandler(IDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<Cabang> Handle(GetCabangQuery request, CancellationToken cancellationToken)
        {
            return _dbContext.Cabang.FirstOrDefault(w => w.kd_cb == request.kd_cb);
        }
    }
}