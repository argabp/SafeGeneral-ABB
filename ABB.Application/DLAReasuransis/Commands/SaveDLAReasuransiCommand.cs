using System;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Helpers;
using ABB.Application.Common.Interfaces;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.DLAReasuransis.Commands
{
    public class SaveDLAReasuransiCommand : IRequest
    {
        
        public string kd_cb { get; set; }

        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public string kd_thn { get; set; }

        public string no_kl { get; set; }

        public short no_mts { get; set; }

        public short no_dla { get; set; }

        public string kd_grp_pas { get; set; }

        public string kd_rk_pas { get; set; }

        public string? ket_dla { get; set; }
    }

    public class SaveDLAReasuransiCommandHandler : IRequestHandler<SaveDLAReasuransiCommand>
    {
        private readonly IDbContextPst _contextPst;
        private readonly ILogger<SaveDLAReasuransiCommandHandler> _logger;

        public SaveDLAReasuransiCommandHandler(IDbContextPst contextPst,
            ILogger<SaveDLAReasuransiCommandHandler> logger, IMapper mapper)
        {;
            _contextPst = contextPst;
            _logger = logger;
        }

        public async Task<Unit> Handle(SaveDLAReasuransiCommand request, CancellationToken cancellationToken)
        {
            await ExceptionHelper.ExecuteWithLoggingAsync(async () => 
            {
                var dlaReasuransi = _contextPst.DLAReasuransi.Find(request.kd_cb, request.kd_cob, request.kd_scob,
                    request.kd_thn, request.no_kl, request.no_mts, request.no_dla);

                dlaReasuransi.ket_dla = request.ket_dla;

                await _contextPst.SaveChangesAsync(cancellationToken);
            }, _logger);

            return Unit.Value;
        }
    }
}