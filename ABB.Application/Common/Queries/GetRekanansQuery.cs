using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using ABB.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using Timer = System.Timers.Timer;

namespace ABB.Application.Common.Queries
{
    public class GetRekanansQuery : IRequest<List<RekananDto>>
    {
        public string DatabaseName { get; set; }
    }

    public class GetRekanansQueryHandler : IRequestHandler<GetRekanansQuery, List<RekananDto>>
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly ILogger<GetRekanansQueryHandler> _logger;

        public GetRekanansQueryHandler(IDbConnectionFactory connectionFactory, ILogger<GetRekanansQueryHandler> logger)
        {
            _connectionFactory = connectionFactory;
            _logger = logger;
        }

        public async Task<List<RekananDto>> Handle(GetRekanansQuery request,
            CancellationToken cancellationToken)
        {
            try
            {
                _connectionFactory.CreateDbConnection(request.DatabaseName);
                
                return (await _connectionFactory.Query<RekananDto>("SELECT kd_cb, kd_grp_rk, kd_rk, nm_rk FROM rf03")).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}