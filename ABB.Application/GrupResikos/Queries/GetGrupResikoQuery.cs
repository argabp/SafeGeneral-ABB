using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.GrupResikos.Queries
{
    public class GetGrupResikoQuery : IRequest<List<GrupResikoDto>>
    {
        public string DatabaseName { get; set; }
        public string SearchKeyword { get; set; }
    }

    public class GetGrupResikoQueryHandler : IRequestHandler<GetGrupResikoQuery, List<GrupResikoDto>>
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly ILogger<GetGrupResikoQueryHandler> _logger;

        public GetGrupResikoQueryHandler(IDbConnectionFactory connectionFactory, ILogger<GetGrupResikoQueryHandler> logger)
        {
            _connectionFactory = connectionFactory;
            _logger = logger;
        }

        public async Task<List<GrupResikoDto>> Handle(GetGrupResikoQuery request,
            CancellationToken cancellationToken)
        {
            await Task.Delay(0, cancellationToken);
            
            try
            {
                _connectionFactory.CreateDbConnection(request.DatabaseName);
                return (await _connectionFactory.Query<GrupResikoDto>("SELECT * FROM rf10")).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}