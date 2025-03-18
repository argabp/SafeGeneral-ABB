using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.GrupResikos.Queries
{
    public class GetDetailGrupResikoQuery : IRequest<List<DetailGrupResikoDto>>
    {
        public string DatabaseName { get; set; }
        public string kd_grp_rsk { get; set; }
    }

    public class GetDetailGrupResikoQueryHandler : IRequestHandler<GetDetailGrupResikoQuery, List<DetailGrupResikoDto>>
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly ILogger<GetDetailGrupResikoQueryHandler> _logger;

        public GetDetailGrupResikoQueryHandler(IDbConnectionFactory connectionFactory, ILogger<GetDetailGrupResikoQueryHandler> logger)
        {
            _connectionFactory = connectionFactory;
            _logger = logger;
        }

        public async Task<List<DetailGrupResikoDto>> Handle(GetDetailGrupResikoQuery request,
            CancellationToken cancellationToken)
        {
            await Task.Delay(0, cancellationToken);

            try
            {
                _connectionFactory.CreateDbConnection(request.DatabaseName);
                return (await _connectionFactory.Query<DetailGrupResikoDto>("SELECT kd_grp_rsk + kd_rsk AS Id, kd_grp_rsk, kd_rsk, " +
                                                                            "desk_rsk, kd_ref, kd_ref1 FROM rf10d WHERE kd_grp_rsk = @kd_grp_rsk",
                    new { request.kd_grp_rsk })).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}