using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Helpers;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.Akseptasis.Queries
{
    public class GeneratePstAndStnRatePremiQuery : IRequest<List<string>>
    {
        public string DatabaseName { get; set; }
        public string kd_cb { get; set; }
        public string kd_cob { get; set; }
        public string kd_scob { get; set; }
        public string kd_thn { get; set; }
        public string no_aks { get; set; }
        public Int16 no_updt { get; set; }
        public Int16 no_rsk { get; set; }
        public string kd_endt { get; set; }
        public string flag_pkk { get; set; }
        public string kd_cvrg { get; set; }
    }

    public class GeneratePstAndStnRatePremiQueryHandler : IRequestHandler<GeneratePstAndStnRatePremiQuery, List<string>>
    {
        private readonly IDbConnectionFactory _connectionFactory;

        private readonly ILogger<GeneratePstAndStnRatePremiQueryHandler> _logger;

        public GeneratePstAndStnRatePremiQueryHandler(IDbConnectionFactory connectionFactory,
            ILogger<GeneratePstAndStnRatePremiQueryHandler> logger)
        {
            _connectionFactory = connectionFactory;
            _logger = logger;
        }

        public async Task<List<string>> Handle(GeneratePstAndStnRatePremiQuery request, CancellationToken cancellationToken)
        {
            return await ExceptionHelper.ExecuteWithLoggingAsync(async () =>
            {
                _connectionFactory.CreateDbConnection(request.DatabaseName);
                return (await _connectionFactory.QueryProc<string>("spe_uw09e02_01", 
                    new
                    {
                        request.kd_cb, request.kd_cob, request.kd_scob, request.kd_thn,
                        request.no_aks, request.no_updt, request.no_rsk, request.kd_endt, 
                    request.flag_pkk, request.kd_cvrg
                })).ToList();
            }, _logger);
        }
    }
}