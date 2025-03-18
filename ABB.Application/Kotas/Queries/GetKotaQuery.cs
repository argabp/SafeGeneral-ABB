using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using ABB.Application.Lokasis.Queries;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.Kotas.Queries
{
    public class GetKotaQuery : IRequest<List<KotaDto>>
    {
        public string DatabaseName { get; set; }
        public string SearchKeyword { get; set; }
    }

    public class GetKotaQueryHandler : IRequestHandler<GetKotaQuery, List<KotaDto>>
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly ILogger<GetKotaQueryHandler> _logger;

        public GetKotaQueryHandler(IDbConnectionFactory connectionFactory, ILogger<GetKotaQueryHandler> logger)
        {
            _connectionFactory = connectionFactory;
            _logger = logger;
        }

        public async Task<List<KotaDto>> Handle(GetKotaQuery request,
            CancellationToken cancellationToken)
        {
            try
            {
                _connectionFactory.CreateDbConnection(request.DatabaseName);
                return (await _connectionFactory.Query<KotaDto>("SELECT * FROM rf07_00")).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}