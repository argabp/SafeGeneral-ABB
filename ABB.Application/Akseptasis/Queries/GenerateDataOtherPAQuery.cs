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
    public class GenerateDataOtherPAQuery : IRequest<List<string>>
    {
        public string DatabaseName { get; set; }
        public string no_pol_ttg { get; set; }
        public string kd_jns_kr { get; set; }
        public string kd_grp_kr { get; set; }
        public string jk_wkt { get; set; }
        public decimal nilai_harga_ptg { get; set; }
        public string flag_std { get; set; }
        public decimal pst_rate_std { get; set; }
        public string flag_bjr { get; set; }
        public decimal pst_rate_bjr { get; set; }
        public string flag_gb { get; set; }
        public decimal pst_rate_gb { get; set; }
        public string flag_tl { get; set; }
        public decimal pst_rate_tl { get; set; }
        public int usia_deb { get; set; }
    }

    public class GenerateDataOtherPAQueryHandler : IRequestHandler<GenerateDataOtherPAQuery, List<string>>
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly ILogger<GenerateDataOtherPAQueryHandler> _logger;
        public GenerateDataOtherPAQueryHandler(IDbConnectionFactory connectionFactory, 
            ILogger<GenerateDataOtherPAQueryHandler> logger)
        {
            _connectionFactory = connectionFactory;
            _logger = logger;
        }

        public async Task<List<string>> Handle(GenerateDataOtherPAQuery request, CancellationToken cancellationToken)
        {
            return await ExceptionHelper.ExecuteWithLoggingAsync(async () =>
            {
                _connectionFactory.CreateDbConnection(request.DatabaseName);
                return (await _connectionFactory.QueryProc<string>("spe_kp02e_07_srm", new
                {
                    kd_cb_pol = request.no_pol_ttg, request.kd_jns_kr, request.kd_grp_kr, request.jk_wkt,
                    request.usia_deb, nilai_jup = request.nilai_harga_ptg, request.flag_std, request.pst_rate_std,
                    request.flag_bjr, request.pst_rate_bjr, request.flag_gb, request.pst_rate_gb,
                    request.flag_tl, request.pst_rate_tl, 
                })).ToList();
            }, _logger);
        }
    }
}