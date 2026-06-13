using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Helpers;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.Alokasis.Commands
{
    public class EndorsSORCommand : IRequest<(string, string, string)>
    {
        public string kd_cb { get; set; }
        public string kd_cob { get; set; }
        public string kd_scob { get; set; }
        public string kd_thn { get; set; }
        public string no_pol { get; set; }
        public string no_updt { get; set; }
        public string no_rsk { get; set; }
        public string kd_endt { get; set; }
        public string no_updt_reas { get; set; }
        public string ket_endt { get; set; }
    }

    public class EndorsSORCommandHandler : IRequestHandler<EndorsSORCommand, (string, string, string)>
    {
        private readonly IDbConnectionPst _dbConnectionPst;
        private readonly ILogger<EndorsSORCommandHandler> _logger;

        public EndorsSORCommandHandler(IDbConnectionPst dbConnectionPst,
            ILogger<EndorsSORCommandHandler> logger)
        {
            _dbConnectionPst = dbConnectionPst;
            _logger = logger;
        }

        public async Task<(string, string, string)> Handle(EndorsSORCommand request, CancellationToken cancellationToken)
        {
            return await ExceptionHelper.ExecuteWithLoggingAsync(async () => 
                (await _dbConnectionPst.QueryProc<(string, string, string)>("spe_ri05e_13x",
                    new
                    {
                        request.kd_cb, request.kd_cob, request.kd_scob,
                        request.kd_thn, request.no_pol, request.no_updt,
                        request.no_rsk, request.kd_endt, request.no_updt_reas,
                        request.ket_endt
                    })).FirstOrDefault(), _logger);
        }
    }
}