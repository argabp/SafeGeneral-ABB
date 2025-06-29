using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.Asumsis.Queries
{
    public class GetAsumsiPeriodeQuery : IRequest<List<AsumsiPeriodeDto>>
    {
        public string KodeAsumsi { get; set; }
    }

    public class GetAsumsiPeriodeQueryHandler : IRequestHandler<GetAsumsiPeriodeQuery, List<AsumsiPeriodeDto>>
    {
        private readonly IDbConnectionCSM _dbConnectionCsm;
        private readonly ILogger<GetAsumsiPeriodeQueryHandler> _logger;

        public GetAsumsiPeriodeQueryHandler(IDbConnectionCSM dbConnectionCsm, ILogger<GetAsumsiPeriodeQueryHandler> logger)
        {
            _dbConnectionCsm = dbConnectionCsm;
            _logger = logger;
        }

        public async Task<List<AsumsiPeriodeDto>> Handle(GetAsumsiPeriodeQuery request,
            CancellationToken cancellationToken)
        {
            await Task.Delay(0, cancellationToken);
            
            var result = new List<AsumsiPeriodeDto>();
            try
            {
                result = (await _dbConnectionCsm.Query<AsumsiPeriodeDto>(
                    "SELECT KodeAsumsi + KodeProduk + CONVERT(VARCHAR, PeriodeProses, 126) AS Id," +
                    "KodeAsumsi, KodeProduk, PeriodeProses " +
                    "FROM MS_AsumsiPeriode WHERE KodeAsumsi = @KodeAsumsi",
                    new { request.KodeAsumsi })).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            return result;
        }
    }
}