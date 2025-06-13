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
    public class GetAsumsiDetailQuery : IRequest<List<AsumsiDetailDto>>
    {
        public string KodeAsumsi { get; set; }

        public string KodeProduk { get; set; }

        public DateTime PeriodeProses { get; set; }
        public string DatabaseName { get; set; }
    }

    public class GetAsumsiDetailQueryHandler : IRequestHandler<GetAsumsiDetailQuery, List<AsumsiDetailDto>>
    {
        private readonly IDbConnectionFactory _dbConnectionFactory;
        private readonly ILogger<GetAsumsiDetailQueryHandler> _logger;

        public GetAsumsiDetailQueryHandler(IDbConnectionFactory dbConnectionFactory, ILogger<GetAsumsiDetailQueryHandler> logger)
        {
            _dbConnectionFactory = dbConnectionFactory;
            _logger = logger;
        }

        public async Task<List<AsumsiDetailDto>> Handle(GetAsumsiDetailQuery request,
            CancellationToken cancellationToken)
        {
            await Task.Delay(0, cancellationToken);
            
            var result = new List<AsumsiDetailDto>();
            try
            {
                var periodeProses = request.PeriodeProses.ToString("yyyy-MM-dd HH:mm:ss");
                
                _dbConnectionFactory.CreateDbConnection(request.DatabaseName);
                result = (await _dbConnectionFactory.Query<AsumsiDetailDto>(
                    "SELECT KodeAsumsi + KodeProduk + CONVERT(VARCHAR, PeriodeProses, 126) + CONVERT(VARCHAR, Thn) AS Id," +
                    "KodeAsumsi, KodeProduk, PeriodeProses, Thn, Persentase" +
                    " FROM MS_AsumsiDetail WHERE KodeAsumsi = @KodeAsumsi" +
                    "   AND KodeProduk = @KodeProduk" +
                    "   AND PeriodeProses = @PeriodeProses",
                    new { request.KodeAsumsi, request.KodeProduk, periodeProses })).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            return result;
        }
    }
}