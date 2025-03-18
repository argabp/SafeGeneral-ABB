using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.PertanggunganKendaraans.Queries
{
    public class GetDetailPertanggunganKendaraansQuery : IRequest<List<DetailPertanggunganKendaraanDto>>
    {
        public string DatabaseName { get; set; }
        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public string kd_jns_ptg { get; set; }
    }

    public class GetDetailPertanggunganKendaraansQueryHandler : IRequestHandler<GetDetailPertanggunganKendaraansQuery, List<DetailPertanggunganKendaraanDto>>
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly ILogger<GetDetailPertanggunganKendaraansQueryHandler> _logger;

        public GetDetailPertanggunganKendaraansQueryHandler(IDbConnectionFactory connectionFactory, ILogger<GetDetailPertanggunganKendaraansQueryHandler> logger)
        {
            _connectionFactory = connectionFactory;
            _logger = logger;
        }

        public async Task<List<DetailPertanggunganKendaraanDto>> Handle(GetDetailPertanggunganKendaraansQuery request,
            CancellationToken cancellationToken)
        {
            await Task.Delay(0, cancellationToken);

            try
            {
                _connectionFactory.CreateDbConnection(request.DatabaseName);
                return (await _connectionFactory.Query<DetailPertanggunganKendaraanDto>("SELECT LTRIM(RTRIM(kd_cob)) + LTRIM(RTRIM(kd_scob)) + LTRIM(RTRIM(kd_jns_ptg)) AS Id, " +
                                                                     "* FROM dp01d WHERE kd_cob = @kd_cob AND " +
                                                                     "kd_scob = @kd_scob AND kd_jns_ptg = @kd_jns_ptg",
                    
                    new { request.kd_cob, request.kd_scob, request.kd_jns_ptg })).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}