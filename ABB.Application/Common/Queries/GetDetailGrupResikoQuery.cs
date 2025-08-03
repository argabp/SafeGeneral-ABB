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
    public class GetDetailGrupResikoQuery : IRequest<List<DropdownOptionDto>>
    {
        public string DatabaseName { get; set; }

        public string kd_grp_rsk { get; set; }
    }

    public class GetDetailGrupResikoQueryHandler : IRequestHandler<GetDetailGrupResikoQuery, List<DropdownOptionDto>>
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly ILogger<GetDetailGrupResikoQueryHandler> _logger;

        public GetDetailGrupResikoQueryHandler(IDbConnectionFactory connectionFactory, ILogger<GetDetailGrupResikoQueryHandler> logger)
        {
            _connectionFactory = connectionFactory;
            _logger = logger;
        }

        public async Task<List<DropdownOptionDto>> Handle(GetDetailGrupResikoQuery request,
            CancellationToken cancellationToken)
        {
            try
            {
                _connectionFactory.CreateDbConnection(request.DatabaseName);
                return (await _connectionFactory.Query<DropdownOptionDto>(
                    "SELECT RTRIM(LTRIM(kd_rsk)) Value, desk_rsk Text " +
                    "FROM rf10d WHERE kd_grp_rsk = @kd_grp_rsk", new { request.kd_grp_rsk })).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}