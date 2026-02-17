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
    public class GetKodeGroupTertanggungPrincipalsQuery : IRequest<List<DropdownOptionDto>>
    {
        public string DatabaseName { get; set; }
    }

    public class GetKodeGroupTertanggungPrincipalsQueryHandler : IRequestHandler<GetKodeGroupTertanggungPrincipalsQuery, List<DropdownOptionDto>>
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly ILogger<GetKodeGroupTertanggungPrincipalsQueryHandler> _logger;

        public GetKodeGroupTertanggungPrincipalsQueryHandler(IDbConnectionFactory connectionFactory, 
            ILogger<GetKodeGroupTertanggungPrincipalsQueryHandler> logger)
        {
            _connectionFactory = connectionFactory;
            _logger = logger;
        }

        public async Task<List<DropdownOptionDto>> Handle(GetKodeGroupTertanggungPrincipalsQuery request,
            CancellationToken cancellationToken)
        {
            return await ExceptionHelper.ExecuteWithLoggingAsync(async () =>
            {
                _connectionFactory.CreateDbConnection(request.DatabaseName);
                return (await _connectionFactory.Query<DropdownOptionDto>("SELECT kd_grp_rk Value, nm_grp_rk Text FROM v_rf02 WHERE nm_grp_rk IN ('Tertanggung', 'Principal')"))
                    .ToList();
            }, _logger);
        }
    }
}