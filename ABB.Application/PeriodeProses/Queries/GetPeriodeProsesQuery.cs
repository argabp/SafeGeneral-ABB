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
        public string DatabaseName { get; set; }
    }

    public class GetPeriodeProsesQueryHandler : IRequestHandler<GetPeriodeProsesQuery, List<PeriodeProsesDto>>
    {
        private readonly IDbContextFactory _dbContextFactory;

        public GetPeriodeProsesQueryHandler(IDbContextFactory dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }

        public async Task<List<PeriodeProsesDto>> Handle(GetPeriodeProsesQuery request,
            CancellationToken cancellationToken)
        {
            await Task.Delay(0, cancellationToken);

            var dbContext = _dbContextFactory.CreateDbContext(request.DatabaseName);
            List<PeriodeProsesModel> periodeProses = dbContext.PeriodeProses.ToList();
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