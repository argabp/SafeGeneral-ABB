using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Dtos;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.Akseptasis.Queries
{
    public class GetLokasiResikoQuery : IRequest<IEnumerable<LokasiResikoDto>>
    {
        public string DatabaseName { get; set; }
    }

    public class GetLokasiResikoQueryHandler : IRequestHandler<GetLokasiResikoQuery, IEnumerable<LokasiResikoDto>>
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly ILogger<GetLokasiResikoQueryHandler> _logger;

        public GetLokasiResikoQueryHandler(IDbConnectionFactory connectionFactory, ILogger<GetLokasiResikoQueryHandler> logger)
        {
            _connectionFactory = connectionFactory;
            _logger = logger;
        }

        public async Task<IEnumerable<LokasiResikoDto>> Handle(GetLokasiResikoQuery request,
            CancellationToken cancellationToken)
        {
            try
            {
                _connectionFactory.CreateDbConnection(request.DatabaseName);
                return (await _connectionFactory.Query<LokasiResikoDto>("SELECT RTRIM(LTRIM(kd_pos)) + LTRIM(RTRIM(kd_lok_rsk)) Id, * FROM rf25d")).AsEnumerable();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}