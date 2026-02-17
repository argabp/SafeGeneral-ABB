using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Dtos;
using ABB.Application.Common.Helpers;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.Akseptasis.Queries
{
    public class GetObligeeQueries : IRequest<List<DropdownOptionDto>>
    {
        public string DatabaseName { get; set; }

        public string kd_grp_rk { get; set; }
    }

    public class GetObligeeQueriesHandler : IRequestHandler<GetObligeeQueries, List<DropdownOptionDto>>
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly ILogger<GetObligeeQueriesHandler> _logger;

        public GetObligeeQueriesHandler(IDbConnectionFactory connectionFactory, ILogger<GetObligeeQueriesHandler> logger)
        {
            _connectionFactory = connectionFactory;
            _logger = logger;
        }

        public async Task<List<DropdownOptionDto>> Handle(GetObligeeQueries request,
            CancellationToken cancellationToken)
        {
            return await ExceptionHelper.ExecuteWithLoggingAsync(async () =>
            {
                _connectionFactory.CreateDbConnection(request.DatabaseName);
                return (await _connectionFactory.Query<DropdownOptionDto>("SELECT RTRIM(LTRIM(kd_rk)) Value, nm_rk Text " +
                                                                          "FROM rf47 WHERE kd_grp_rk = @kd_grp_rk", new { request.kd_grp_rk })).ToList();
            }, _logger);
        }
    }
}