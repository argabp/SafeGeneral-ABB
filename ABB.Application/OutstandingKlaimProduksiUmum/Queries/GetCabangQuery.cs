using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using ABB.Application.Common.Services;
using MediatR;

namespace ABB.Application.OutstandingKlaimProduksiUmum.Queries
{
    public class GetCabangQuery : IRequest<List<CabangDto>>
    {
    }

    public class GetCabangQueryHandler : IRequestHandler<GetCabangQuery, List<CabangDto>>
    {
        private readonly IDbConnectionCSM _db;

        public GetCabangQueryHandler(IDbConnectionCSM db)
        {
            _db = db;
        }

        public async Task<List<CabangDto>> Handle(GetCabangQuery request,
            CancellationToken cancellationToken)
        {
            var result =
                await _db.Query<CabangDto>("SELECT * FROM rf01");

            return result.ToList();
        }
    }
}