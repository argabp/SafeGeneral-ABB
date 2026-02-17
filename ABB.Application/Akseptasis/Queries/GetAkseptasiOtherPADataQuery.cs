using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Helpers;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.Akseptasis.Queries
{
    public class GetAkseptasiOtherPADataQuery : IRequest<List<string>>
    {
        public string DatabaseName { get; set; }
        public decimal jk_wkt { get; set; }
        public DateTime tgl_lahir { get; set; }
        public DateTime tgl_real { get; set; }
        public string kd_cb { get; set; }
        public string kd_jns_kr { get; set; }
        public string kd_grp_kr { get; set; }
    }

    public class GetAkseptasiOtherPADataQueryHandler : IRequestHandler<GetAkseptasiOtherPADataQuery, List<string>>
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly ILogger<GetAkseptasiOtherPADataQueryHandler> _logger;

        public GetAkseptasiOtherPADataQueryHandler(IDbConnectionFactory connectionFactory,
            ILogger<GetAkseptasiOtherPADataQueryHandler> logger)
        {
            _connectionFactory = connectionFactory;
            _logger = logger;
        }

        public async Task<List<string>> Handle(GetAkseptasiOtherPADataQuery request, CancellationToken cancellationToken)
        {
            return await ExceptionHelper.ExecuteWithLoggingAsync(async () =>
            {
                _connectionFactory.CreateDbConnection(request.DatabaseName);
                var results = (await _connectionFactory.QueryProc<string>("spe_kp02e_04", 
                    new { request.jk_wkt, request.tgl_lahir, request.tgl_real, request.kd_cb, request.kd_jns_kr, request.kd_grp_kr })).ToList();
                
                for (int i = 0; i < results.Count; i++)
                {
                    var parts = results[i].Split(',');

                    if (parts.Length == 3 && parts[2] == "DT" && !string.IsNullOrWhiteSpace(parts[1]))
                    {
                        if (DateTime.TryParseExact(parts[1], "yyyy/MM/dd",
                                CultureInfo.InvariantCulture,
                                DateTimeStyles.None,
                                out DateTime dt))
                        {
                            parts[1] = dt.ToString("dd/MM/yyyy");
                        }

                        results[i] = string.Join(",", parts);
                    }
                }

                return results;
            }, _logger);
        }
    }
}