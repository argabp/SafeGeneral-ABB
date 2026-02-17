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
    public class GetKodeKodeOkupasiQuery : IRequest<List<DropdownOptionDto>>
    {
        public string DatabaseName { get; set; }
    }

    public class GetKodeKodeOkupasiQueryHandler : IRequestHandler<GetKodeKodeOkupasiQuery, List<DropdownOptionDto>>
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly ILogger<GetKodeKodeOkupasiQueryHandler> _logger;

        public GetKodeKodeOkupasiQueryHandler(IDbConnectionFactory connectionFactory, ILogger<GetKodeKodeOkupasiQueryHandler> logger)
        {
            _connectionFactory = connectionFactory;
            _logger = logger;
        }

        public async Task<List<DropdownOptionDto>> Handle(GetKodeKodeOkupasiQuery request,
            CancellationToken cancellationToken)
        {
            return await ExceptionHelper.ExecuteWithLoggingAsync(async () =>
            {
                _connectionFactory.CreateDbConnection(request.DatabaseName);
                return (await _connectionFactory.Query<DropdownOptionDto>("SELECT RTRIM(LTRIM(kd_okup)) Value, '(' + RTRIM(LTRIM(kd_okup)) + ') ' + nm_okup Text " +
                                                                          "FROM rf11")).ToList();
            }, _logger);
        }
    }
}