using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.Okupasis.Queries
{
    public class GetDetailOkupasiQuery : IRequest<List<DetailOkupasiDto>>
    {
        public string DatabaseName { get; set; }
        public string kd_okup { get; set; }
    }

    public class GetDetailOkupasiQueryHandler : IRequestHandler<GetDetailOkupasiQuery, List<DetailOkupasiDto>>
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly ILogger<GetDetailOkupasiQueryHandler> _logger;

        public GetDetailOkupasiQueryHandler(IDbConnectionFactory connectionFactory, ILogger<GetDetailOkupasiQueryHandler> logger)
        {
            _connectionFactory = connectionFactory;
            _logger = logger;
        }

        public async Task<List<DetailOkupasiDto>> Handle(GetDetailOkupasiQuery request,
            CancellationToken cancellationToken)
        {
            await Task.Delay(0, cancellationToken);

            try
            {
                _connectionFactory.CreateDbConnection(request.DatabaseName);
                return (await _connectionFactory.Query<DetailOkupasiDto>("SELECT kd_okup + kd_kls_konstr AS Id," +
                                                                         "kd_okup, kd_kls_konstr, stn_rate_prm, pst_rate_prm" +
                                                                         " FROM rf11d WHERE kd_okup = @kd_okup",
                    new { request.kd_okup })).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}