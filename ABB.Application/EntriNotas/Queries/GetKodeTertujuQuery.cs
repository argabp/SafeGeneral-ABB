using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Dtos;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.EntriNotas.Queries
{
    public class GetKodeTertujuQuery : IRequest<List<DropdownOptionDto>>
    {
        public string DatabaseName { get; set; }
    }

    public class GetKodeTertujuQueryHandler : IRequestHandler<GetKodeTertujuQuery, List<DropdownOptionDto>>
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly ILogger<GetKodeTertujuQueryHandler> _logger;

        public GetKodeTertujuQueryHandler(IDbConnectionFactory connectionFactory, ILogger<GetKodeTertujuQueryHandler> logger)
        {
            _connectionFactory = connectionFactory;
            _logger = logger;
        }

        public async Task<List<DropdownOptionDto>> Handle(GetKodeTertujuQuery request,
            CancellationToken cancellationToken)
        {
            try
            {
                _connectionFactory.CreateDbConnection(request.DatabaseName);
                return (await _connectionFactory.Query<DropdownOptionDto>("SELECT RTRIM(LTRIM(kd_grp_rk)) Value, nm_grp_rk Text " +
                                                                          "FROM v_rf02")).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}