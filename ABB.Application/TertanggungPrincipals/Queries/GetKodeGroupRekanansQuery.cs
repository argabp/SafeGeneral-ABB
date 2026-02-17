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

namespace ABB.Application.TertanggungPrincipals.Queries
{
    public class GetKodeGroupRekanansQuery : IRequest<List<DropdownOptionDto>>
    {
        public string DatabaseName { get; set; }
    }

    public class GetKodeGroupRekanansQueryHandler : IRequestHandler<GetKodeGroupRekanansQuery, List<DropdownOptionDto>>
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly ILogger<GetKodeGroupRekanansQueryHandler> _logger;

        public GetKodeGroupRekanansQueryHandler(IDbConnectionFactory connectionFactory, 
            ILogger<GetKodeGroupRekanansQueryHandler> logger)
        {
            _connectionFactory = connectionFactory;
            _logger = logger;
        }

        public async Task<List<DropdownOptionDto>> Handle(GetKodeGroupRekanansQuery request,
            CancellationToken cancellationToken)
        {
            return await ExceptionHelper.ExecuteWithLoggingAsync(async () =>
            {
                _connectionFactory.CreateDbConnection(request.DatabaseName);
                return (await _connectionFactory.Query<DropdownOptionDto>("SELECT kd_grp_rk Value, nm_grp_rk Text FROM v_rf02 WHERE kd_grp_rk in ('6','9', 'P')"))
                    .ToList();
            }, _logger);
        }
    }
}