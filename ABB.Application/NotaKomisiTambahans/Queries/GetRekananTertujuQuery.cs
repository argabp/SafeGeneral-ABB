using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Dtos;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.NotaKomisiTambahans.Queries
{
    public class GetRekananTertujuQuery : IRequest<List<DropdownOptionDto>>
    {
        public string DatabaseName { get; set; }

        public string kd_grp_rk { get; set; }
    }

    public class GetRekananTertujuQueryHandler : IRequestHandler<GetRekananTertujuQuery, List<DropdownOptionDto>>
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly ILogger<GetRekananTertujuQueryHandler> _logger;

        public GetRekananTertujuQueryHandler(IDbConnectionFactory connectionFactory, ILogger<GetRekananTertujuQueryHandler> logger)
        {
            _connectionFactory = connectionFactory;
            _logger = logger;
        }

        public async Task<List<DropdownOptionDto>> Handle(GetRekananTertujuQuery request,
            CancellationToken cancellationToken)
        {
            try
            {
                _connectionFactory.CreateDbConnection(request.DatabaseName);
                return (await _connectionFactory.Query<DropdownOptionDto>("SELECT RTRIM(LTRIM(kd_rk)) Value, nm_rk Text " +
                                                                          "FROM rf03 WHERE kd_grp_rk = @kd_grp_rk", new { request.kd_grp_rk } )).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}