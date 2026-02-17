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
    public class GetGroupObligeeQuery : IRequest<List<DropdownOptionDto>>
    {
        public string DatabaseName { get; set; }

        public string grp_obl { get; set; }
    }

    public class GetGroupObligeeQueryHandler : IRequestHandler<GetGroupObligeeQuery, List<DropdownOptionDto>>
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly ILogger<GetGroupObligeeQueryHandler> _logger;

        public GetGroupObligeeQueryHandler(IDbConnectionFactory connectionFactory, ILogger<GetGroupObligeeQueryHandler> logger)
        {
            _connectionFactory = connectionFactory;
            _logger = logger;
        }

        public async Task<List<DropdownOptionDto>> Handle(GetGroupObligeeQuery request,
            CancellationToken cancellationToken)
        {
            return await ExceptionHelper.ExecuteWithLoggingAsync(async () =>
            {
                _connectionFactory.CreateDbConnection(request.DatabaseName);
                return (await _connectionFactory.Query<DropdownOptionDto>("SELECT RTRIM(LTRIM(kd_grp_rsk)) Value, desk_grp_rsk Text " +
                                                                          "FROM rf10 WHERE grp_obl = @grp_obl", new { request.grp_obl })).ToList();
            }, _logger);
        }
    }
}