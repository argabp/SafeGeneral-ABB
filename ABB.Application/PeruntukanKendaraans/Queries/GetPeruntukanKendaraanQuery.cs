using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.PeruntukanKendaraans.Queries
{
    public class GetPeruntukanKendaraanQuery : IRequest<List<PeruntukanKendaraanDto>>
    {
        public string SearchKeyword { get; set; }
        public string DatabaseName { get; set; }
    }

    public class GetPeruntukanKendaraanQueryHandler : IRequestHandler<GetPeruntukanKendaraanQuery, List<PeruntukanKendaraanDto>>
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly ILogger<GetPeruntukanKendaraanQueryHandler> _logger;

        public GetPeruntukanKendaraanQueryHandler(IDbConnectionFactory connectionFactory, ILogger<GetPeruntukanKendaraanQueryHandler> logger)
        {
            _connectionFactory = connectionFactory;
            _logger = logger;
        }

        public async Task<List<PeruntukanKendaraanDto>> Handle(GetPeruntukanKendaraanQuery request,
            CancellationToken cancellationToken)
        {
            try
            {
                _connectionFactory.CreateDbConnection(request.DatabaseName);
                return (await _connectionFactory.Query<PeruntukanKendaraanDto>("SELECT * FROM rf16")).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}