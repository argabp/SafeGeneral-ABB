using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Dtos;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.Inquiries.Queries
{
    public class GetAsuransiJiwaQuery : IRequest<List<DropdownOptionDto>>
    {
        public string DatabaseName { get; set; }

        public string kd_grp_asj { get; set; }
    }

    public class GetAsuransiJiwaQueryHandler : IRequestHandler<GetAsuransiJiwaQuery, List<DropdownOptionDto>>
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly ILogger<GetAsuransiJiwaQueryHandler> _logger;

        public GetAsuransiJiwaQueryHandler(IDbConnectionFactory connectionFactory, ILogger<GetAsuransiJiwaQueryHandler> logger)
        {
            _connectionFactory = connectionFactory;
            _logger = logger;
        }

        public async Task<List<DropdownOptionDto>> Handle(GetAsuransiJiwaQuery request,
            CancellationToken cancellationToken)
        {
            try
            {
                _connectionFactory.CreateDbConnection(request.DatabaseName);
                return (await _connectionFactory.Query<DropdownOptionDto>(
                    "SELECT RTRIM(LTRIM(kd_rk)) Value, nm_rk Text " +
                    "FROM rf03 WHERE kd_grp_rk = @kd_grp_asj", 
                    new { request.kd_grp_asj })).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}