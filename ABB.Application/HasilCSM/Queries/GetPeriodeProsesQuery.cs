using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;

namespace ABB.Application.HasilCSM.Queries
{
    public class GetPeriodeProsesQuery : IRequest<List<PeriodeProsesDto>>
    {
    }

    public class GetPeriodeProsesQueryHandler : IRequestHandler<GetPeriodeProsesQuery, List<PeriodeProsesDto>>
    {
        private readonly IDbConnectionCSM _db;

        public GetPeriodeProsesQueryHandler(IDbConnectionCSM db)
        {
            _db = db;
        }

        public async Task<List<PeriodeProsesDto>> Handle(GetPeriodeProsesQuery request,
            CancellationToken cancellationToken)
        {
            var result =
                await _db.Query<PeriodeProsesDto>($"SELECT CAST(PeriodeProses AS varchar(11)) AS Text, CAST(PeriodeProses AS varchar(11)) AS Value FROM MS_PeriodeProses");

            return result.ToList();
        }
    }
}