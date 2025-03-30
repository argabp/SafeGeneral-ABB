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
    public class GetRekananTertujuQuery : IRequest<List<DropdownOptionDto>>
    {
        public string DatabaseName { get; set; }

        public string kd_cb { get; set; }
        public string kd_cob { get; set; }
        public string kd_scob { get; set; }
        public string kd_thn { get; set; }
        public string no_pol { get; set; }
        public Int16 no_updt { get; set; }
        public string kd_grp_ttj { get; set; }
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
                                                                          "FROM v_uw08e_nota WHERE kd_cb = @kd_cb AND " +
                                                                          "kd_cob = @kd_cob AND kd_scob = @kd_scob AND " +
                                                                          "kd_thn = @kd_thn AND no_pol = @no_pol AND " +
                                                                          "no_updt = @no_updt AND kd_grp_rk = @kd_grp_rk", 
                    new { request.kd_cb, request.kd_cob, request.kd_scob,
                        request.kd_thn, request.no_pol, request.no_updt, kd_grp_rk = request.kd_grp_ttj })).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}