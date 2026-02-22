using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Helpers;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.KlaimAlokasiReasuransis.Commands
{
    public class AlokasiReasCommand : IRequest<(string, string, string)>
    {
        public string DatabaseName { get; set; }
        public string kd_cb { get; set; }
        public string kd_cob { get; set; }
        public string kd_scob { get; set; }
        public string kd_thn { get; set; }
        public string no_kl { get; set; }
        public Int16 no_mts { get; set; }

        public DateTime? tgl_closing { get; set; }

        public string kd_usr_input { get; set; }
    }

    public class AlokasiReasCommandHandler : IRequestHandler<AlokasiReasCommand, (string, string, string)>
    {
        private readonly IDbConnectionPstNota _connectionPst;
        private readonly ILogger<AlokasiReasCommandHandler> _logger;

        public AlokasiReasCommandHandler(IDbConnectionPstNota connectionPst,
            ILogger<AlokasiReasCommandHandler> logger)
        {
            _connectionPst = connectionPst;
            _logger = logger;
        }

        public async Task<(string, string, string)> Handle(AlokasiReasCommand request, CancellationToken cancellationToken)
        {
            return await ExceptionHelper.ExecuteWithLoggingAsync(async () =>
            {
                return (await _connectionPst.QueryProc<(string, string, string)>("spp_cl03e_01",
                    new
                    {
                        request.kd_cb, request.kd_cob, request.kd_scob,
                        request.kd_thn, request.no_kl, request.no_mts
                    })).FirstOrDefault();
            }, _logger);
        }
    }
}