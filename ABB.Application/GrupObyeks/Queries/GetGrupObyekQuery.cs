using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.GrupObyeks.Queries
{
    public class GetGrupObyekQuery : IRequest<List<GrupObyekDto>>
    {
        public string DatabaseName { get; set; }
        public string SearchKeyword { get; set; }
    }

    public class GetGrupObyekQueryHandler : IRequestHandler<GetGrupObyekQuery, List<GrupObyekDto>>
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly ILogger<GetGrupObyekQueryHandler> _logger;

        public GetGrupObyekQueryHandler(IDbConnectionFactory connectionFactory, ILogger<GetGrupObyekQueryHandler> logger)
        {
            _connectionFactory = connectionFactory;
            _logger = logger;
        }

        public async Task<List<GrupObyekDto>> Handle(GetGrupObyekQuery request,
            CancellationToken cancellationToken)
        {
            try
            {
                _connectionFactory.CreateDbConnection(request.DatabaseName);
                return (await _connectionFactory.Query<GrupObyekDto>("SELECT * FROM rf14")).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}