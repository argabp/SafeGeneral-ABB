using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Helpers;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.ProsesPremiXOLKeluars.Commands
{
    public class ProsesPremiXOLKeluarCommand : IRequest<(string, string, string)>
    {
        public string kd_cb { get; set; }

        public string kd_thn { get; set; }

        public string kd_bln { get; set; }

        public string kd_jns_sor { get; set; }

        public string kd_tty_npps { get; set; }

        public string kd_mtu { get; set; }

        public string no_tr { get; set; }

        public DateTime tgl_closing { get; set; }
    }

    public class ProsesPremiXOLKeluarCommandHandler : IRequestHandler<ProsesPremiXOLKeluarCommand, (string, string, string)>
    {
        private readonly IDbConnectionPst _connectionPst;
        private readonly ILogger<ProsesPremiXOLKeluarCommandHandler> _logger;

        public ProsesPremiXOLKeluarCommandHandler(IDbConnectionPst connectionPst,
            ILogger<ProsesPremiXOLKeluarCommandHandler> logger)
        {
            _connectionPst = connectionPst;
            _logger = logger;
        }

        public async Task<(string, string, string)> Handle(ProsesPremiXOLKeluarCommand request, CancellationToken cancellationToken)
        {
            return await ExceptionHelper.ExecuteWithLoggingAsync(async () => (await _connectionPst.QueryProc<(string, string, string)>("spp_ri09p_02",
                new
                {
                    request.kd_cb, request.kd_bln, request.kd_jns_sor,
                    request.kd_thn, request.kd_tty_npps, request.kd_mtu,
                    request.tgl_closing, request.no_tr
                })).FirstOrDefault(), _logger);
        }
    }
}