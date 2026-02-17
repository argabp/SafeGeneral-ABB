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
    public class GetKodeJenisKreditQuery : IRequest<List<DropdownOptionDto>>
    {
        public string DatabaseName { get; set; }

        public string kd_cb { get; set; }
    }

    public class GetKodeJenisKreditQueryHandler : IRequestHandler<GetKodeJenisKreditQuery, List<DropdownOptionDto>>
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly ILogger<GetKodeJenisKreditQueryHandler> _logger;

        public GetKodeJenisKreditQueryHandler(IDbConnectionFactory connectionFactory, ILogger<GetKodeJenisKreditQueryHandler> logger)
        {
            _connectionFactory = connectionFactory;
            _logger = logger;
        }

        public async Task<List<DropdownOptionDto>> Handle(GetKodeJenisKreditQuery request,
            CancellationToken cancellationToken)
        {
            return await ExceptionHelper.ExecuteWithLoggingAsync(async () =>
            {
                _connectionFactory.CreateDbConnection(request.DatabaseName);
                return (await _connectionFactory.Query<DropdownOptionDto>(
                    "SELECT RTRIM(LTRIM(kd_jns_kr)) Value, nm_jns_kr Text " +
                    "FROM rf02 WHERE kd_cb = @kd_cb", new { request.kd_cb })).ToList();
            }, _logger);
        }
    }
}