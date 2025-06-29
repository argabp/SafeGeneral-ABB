using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using ABB.Application.Common.Services;
using ABB.Domain.Entities;
using MediatR;

namespace ABB.Application.PeriodeProses.Queries
{
    public class GetPeriodeProsesQuery : IRequest<List<PeriodeProsesDto>>
    {
    }

    public class GetPeriodeProsesQueryHandler : IRequestHandler<GetPeriodeProsesQuery, List<PeriodeProsesDto>>
    {
        private readonly IDbContextCSM _dbContextCsm;

        public GetPeriodeProsesQueryHandler(IDbContextCSM dbContextCsm)
        {
            _dbContextCsm = dbContextCsm;
        }

        public async Task<List<PeriodeProsesDto>> Handle(GetPeriodeProsesQuery request,
            CancellationToken cancellationToken)
        {
            await Task.Delay(0, cancellationToken);

            List<PeriodeProsesModel> periodeProses = _dbContextCsm.PeriodeProses.ToList();
            List<PeriodeProsesDto> periodeProsesDtos = new List<PeriodeProsesDto>();

            for (int sequence = 0; sequence < periodeProses.Count; sequence++)
            {
                periodeProsesDtos.Add(new PeriodeProsesDto()
                {
                    Id = sequence + 1,
                    PeriodeProses = periodeProses[sequence].PeriodeProses,
                    FlagProses = periodeProses[sequence].FlagProses
                });
            }

            return periodeProsesDtos;
        }
    }
}