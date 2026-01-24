using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Dtos;
using ABB.Application.Common.Interfaces;
using ABB.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using Timer = System.Timers.Timer;

namespace ABB.Application.Common.Queries
{
    public class GetRekanansByKodeGroupQuery : IRequest<List<DropdownOptionDto>>
    {
        public string DatabaseName { get; set; }

        public string kd_grp_rk { get; set; }
    }

    public class GetRekanansByKodeGroupQueryHandler : IRequestHandler<GetRekanansByKodeGroupQuery, List<DropdownOptionDto>>
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly ILogger<GetRekanansByKodeGroupQueryHandler> _logger;

        public GetRekanansByKodeGroupQueryHandler(IDbConnectionFactory connectionFactory, ILogger<GetRekanansByKodeGroupQueryHandler> logger)
        {
            _connectionFactory = connectionFactory;
            _logger = logger;
        }

        public async Task<List<DropdownOptionDto>> Handle(GetRekanansByKodeGroupQuery request,
            CancellationToken cancellationToken)
        {
            try
            {
                _connectionFactory.CreateDbConnection(request.DatabaseName);

                return (await _connectionFactory.Query<DropdownOptionDto>(
                    "SELECT kd_rk Value, nm_rk Text FROM rf03 WHERE kd_grp_rk = @kd_grp_rk",
                    new { request.kd_grp_rk })).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}