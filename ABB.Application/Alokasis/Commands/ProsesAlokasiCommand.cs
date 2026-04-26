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
    public class ProsesAlokasiCommand : IRequest<string>
    {
        public string kd_cb { get; set; }
        public string kd_cob { get; set; }
        public string kd_scob { get; set; }
        public string kd_thn { get; set; }
        public string no_pol { get; set; }
        public string no_updt { get; set; }
        public DateTime tgl_closing { get; set; }
        public string st_tty { get; set; }
        public string flag_survey { get; set; }
    }

    public class ProsesAlokasiCommandHandler : IRequestHandler<ProsesAlokasiCommand, string>
    {
        private readonly IDbConnectionPst _dbConnectionPst;
        private readonly ILogger<ProsesAlokasiCommandHandler> _logger;

        public ProsesAlokasiCommandHandler(IDbConnectionPst dbConnectionPst,
            ILogger<ProsesAlokasiCommandHandler> logger)
        {
            _dbConnectionPst = dbConnectionPst;
            _logger = logger;
        }

        public async Task<string> Handle(ProsesAlokasiCommand request, CancellationToken cancellationToken)
        {
            return await ExceptionHelper.ExecuteWithLoggingAsync(async () => 
                (await _dbConnectionPst.QueryProc<string>("spp_ri04p_07",
                new
                {
                    request.kd_cb, request.kd_cob, request.kd_scob,
                    request.kd_thn, request.no_pol, request.no_updt,
                    tgl_closing_reas = request.tgl_closing, request.st_tty, request.flag_survey
                })).FirstOrDefault(), _logger);
        }
    }
}