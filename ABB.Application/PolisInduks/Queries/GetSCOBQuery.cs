using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Dtos;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.PolisInduks.Queries
{
    public class GetSCOBQuery : IRequest<List<DropdownOptionDto>>
    {
        public string DatabaseName { get; set; }
        public string SearchKeyword { get; set; }
        public string kd_cob { get; set; }
    }

    public class GetSCOBQueryHandler : IRequestHandler<GetSCOBQuery, List<DropdownOptionDto>>
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly ILogger<GetSCOBQueryHandler> _logger;

        public GetSCOBQueryHandler(IDbConnectionFactory connectionFactory, ILogger<GetSCOBQueryHandler> logger)
        {
            _connectionFactory = connectionFactory;
            _logger = logger;
        }

        public async Task<List<DropdownOptionDto>> Handle(GetSCOBQuery request,
            CancellationToken cancellationToken)
        {
            try
            {
                _connectionFactory.CreateDbConnection(request.DatabaseName);
                return (await _connectionFactory.Query<DropdownOptionDto>(
                    @"SELECT RTRIM(LTRIM(kd_scob)) Value, nm_scob Text FROM rf05 WHERE kd_cob = @kd_cob",
                    new { request.kd_cob })).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}