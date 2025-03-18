using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.Obligees.Queries
{
    public class GetObligeesQuery : IRequest<List<ObligeeDto>>
    {
        public string DatabaseName { get; set; }
        public string SearchKeyword { get; set; }
        public string KodeCabang { get; set; }
    }

    public class GetObligeesQueryHandler : IRequestHandler<GetObligeesQuery, List<ObligeeDto>>
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly ILogger<GetObligeesQueryHandler> _logger;

        public GetObligeesQueryHandler(IDbConnectionFactory connectionFactory, ILogger<GetObligeesQueryHandler> logger)
        {
            _connectionFactory = connectionFactory;
            _logger = logger;
        }

        public async Task<List<ObligeeDto>> Handle(GetObligeesQuery request,
            CancellationToken cancellationToken)
        {
            try
            {
                _connectionFactory.CreateDbConnection(request.DatabaseName);
                var obligees = (await _connectionFactory.Query<ObligeeDto>(@"SELECT r.kd_cb + r.kd_grp_rk + r.kd_rk AS Id,
                r.kd_cb, r.kd_grp_rk, r.kd_rk,
                c.nm_cb, g.nm_grp_rk, k.nm_kota,
                r.kd_kota, r.nm_rk, r.almt, r.flag_sic
                    FROM rf47 r
                INNER JOIN rf01 c
                ON r.kd_cb = c.kd_cb
                INNER JOIN v_rf02 g
                ON r.kd_grp_rk = g.kd_grp_rk
                INNER JOIN rf07_00 k
                ON r.kd_kota = k.kd_kota
                WHERE c.kd_cb = @KodeCabang", new { request.KodeCabang })).ToList();

                foreach (var obligee in obligees)
                    obligee.nm_sic = obligee.flag_sic == "R" ? "Retail" : "Corporate";

                return obligees;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}