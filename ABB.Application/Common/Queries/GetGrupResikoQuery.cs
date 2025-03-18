using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Dtos;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.Common.Queries
{
    public class GetGrupResikoQuery : IRequest<List<DropdownOptionDto>>
    {
        public string DatabaseName { get; set; }

        public string kd_jns_grp { get; set; }
    }

    public class GetGrupResikoQueryHandler : IRequestHandler<GetGrupResikoQuery, List<DropdownOptionDto>>
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly ILogger<GetGrupResikoQueryHandler> _logger;

        public GetGrupResikoQueryHandler(IDbConnectionFactory connectionFactory, ILogger<GetGrupResikoQueryHandler> logger)
        {
            _connectionFactory = connectionFactory;
            _logger = logger;
        }

        public async Task<List<DropdownOptionDto>> Handle(GetGrupResikoQuery request,
            CancellationToken cancellationToken)
        {
            try
            {
                _connectionFactory.CreateDbConnection(request.DatabaseName);
                
                if(string.IsNullOrWhiteSpace(request.kd_jns_grp))
                    return (await _connectionFactory.Query<DropdownOptionDto>(
                        "SELECT RTRIM(LTRIM(kd_grp_rsk)) Value, desk_grp_rsk Text " +
                        "FROM rf10")).ToList();
                
                return (await _connectionFactory.Query<DropdownOptionDto>(
                    "SELECT RTRIM(LTRIM(kd_grp_rsk)) Value, desk_grp_rsk Text " +
                    "FROM rf10 WHERE kd_jns_grp = @kd_jns_grp", new { request.kd_jns_grp })).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}