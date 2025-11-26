using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Dtos;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.RegisterKlaims.Queries
{
    public class GetKodeSebabQuery : IRequest<List<DropdownOptionDto>>
    {
        public string DatabaseName { get; set; }

        public string kd_cob { get; set; }
    }

    public class GetKodeSebabQueryHandler : IRequestHandler<GetKodeSebabQuery, List<DropdownOptionDto>>
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly ILogger<GetKodeSebabQueryHandler> _logger;

        public GetKodeSebabQueryHandler(IDbConnectionFactory connectionFactory, ILogger<GetKodeSebabQueryHandler> logger)
        {
            _connectionFactory = connectionFactory;
            _logger = logger;
        }

        public async Task<List<DropdownOptionDto>> Handle(GetKodeSebabQuery request,
            CancellationToken cancellationToken)
        {
            try
            {
                _connectionFactory.CreateDbConnection(request.DatabaseName);
                return (await _connectionFactory.Query<DropdownOptionDto>(
                    "SELECT RTRIM(LTRIM(kd_sebab)) Value, nm_sebab Text " +
                    "FROM v_rf01 WHERE kd_cob = @kd_cob", new { request.kd_cob })).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}