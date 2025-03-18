using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using ABB.Application.Rekanans.Queries;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.TertanggungPrincipals.Queries
{
    public class GetTertanggungPrincipalsQuery : IRequest<List<RekananDto>>
    {
        public string DatabaseName { get; set; }
        public string SearchKeyword { get; set; }

        public string ModuleType { get; set; }

        public string KodeCabang { get; set; }
    }

    public class GetTertanggungPrincipalsQueryHandler : IRequestHandler<GetTertanggungPrincipalsQuery, List<RekananDto>>
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly IDbContextFactory _contextFactory;
        private readonly ILogger<GetTertanggungPrincipalsQueryHandler> _logger;

        public GetTertanggungPrincipalsQueryHandler(IDbConnectionFactory connectionFactory, 
            IDbContextFactory contextFactory, ILogger<GetTertanggungPrincipalsQueryHandler> logger)
        {
            _connectionFactory = connectionFactory;
            _contextFactory = contextFactory;
            _logger = logger;
        }

        public async Task<List<RekananDto>> Handle(GetTertanggungPrincipalsQuery request,
            CancellationToken cancellationToken)
        {
            try
            {
                var dbContext = _contextFactory.CreateDbContext(request.DatabaseName);
                var module = dbContext.Module.Find(int.Parse(request.ModuleType));
                string suffix = string.Empty;
                if(module != null)
                    switch (module.Name.ToLower())
                    {
                        case "underwriting":
                            suffix += " AND g.nm_grp_rk IN ('Tertanggung', 'Principal')";
                            break;
                    }
                
                _connectionFactory.CreateDbConnection(request.DatabaseName);
                var rekanans = (await _connectionFactory.Query<RekananDto>(@"SELECT r.kd_cb + r.kd_grp_rk + r.kd_rk AS Id,
                r.kd_cb, r.kd_grp_rk, r.kd_rk,
                c.nm_cb, g.nm_grp_rk, k.nm_kota,
                r.kd_kota, r.nm_rk, r.almt, r.flag_sic
                    FROM rf03 r
                INNER JOIN rf01 c
                ON r.kd_cb = c.kd_cb
                INNER JOIN v_rf02 g
                ON r.kd_grp_rk = g.kd_grp_rk
                INNER JOIN rf07_00 k
                ON r.kd_kota = k.kd_kota
                WHERE c.kd_cb = @KodeCabang" + suffix, new { request.KodeCabang })).ToList();

                foreach (var rekanan in rekanans)
                    rekanan.nm_sic = rekanan.flag_sic == "R" ? "Retail" : "Corporate";

                return rekanans;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}