using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.Common.Queries
{
    public class GetAllSCOBQuery : IRequest<List<SCOBDto>>
    {
        public string DatabaseName { get; set; }
    }

    public class GetAllSCOBQueryHandler : IRequestHandler<GetAllSCOBQuery, List<SCOBDto>>
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly ILogger<GetAllSCOBQueryHandler> _logger;

        public GetAllSCOBQueryHandler(IDbConnectionFactory connectionFactory, ILogger<GetAllSCOBQueryHandler> logger)
        {
            _connectionFactory = connectionFactory;
            _logger = logger;
        }

        public async Task<List<SCOBDto>> Handle(GetAllSCOBQuery request,
            CancellationToken cancellationToken)
        {
            try
            {
                _connectionFactory.CreateDbConnection(request.DatabaseName);
                return (await _connectionFactory.Query<SCOBDto>(
                    @"SELECT RTRIM(LTRIM(kd_scob)) kd_scob, nm_scob, RTRIM(LTRIM(kd_cob)) kd_cob FROM rf05")).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}