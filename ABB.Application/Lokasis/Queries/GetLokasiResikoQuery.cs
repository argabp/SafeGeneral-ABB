using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.Lokasis.Queries
{
    public class GetLokasiResikoQuery : IRequest<List<LokasiResikoDto>>
    {
        public string DatabaseName { get; set; }
        public string SearchKeyword { get; set; }
    }

    public class GetLokasiResikoQueryHandler : IRequestHandler<GetLokasiResikoQuery, List<LokasiResikoDto>>
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly ILogger<GetLokasiResikoQueryHandler> _logger;

        public GetLokasiResikoQueryHandler(IDbConnectionFactory connectionFactory, ILogger<GetLokasiResikoQueryHandler> logger)
        {
            _connectionFactory = connectionFactory;
            _logger = logger;
        }

        public async Task<List<LokasiResikoDto>> Handle(GetLokasiResikoQuery request,
            CancellationToken cancellationToken)
        {
            try
            {
                _connectionFactory.CreateDbConnection(request.DatabaseName);
                return (await _connectionFactory.Query<LokasiResikoDto>("SELECT * FROM rf25")).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}